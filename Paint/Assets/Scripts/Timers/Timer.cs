﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// Timer is both static pool manager and dynamic DI container for concrete behaviors
/// </summary>
public partial class Timer
{
    /// <summary>
    /// Implementers create behavior
    /// </summary>
    public interface ITimerBehavior
    {
        void Initialize();
        void Update(float deltaTime);
    }


    public bool Paused { get; private set; }
    //DI defines what kind of timer this is
    ITimerBehavior behavior;
    //This base is shared between all types of timers
    TimerBehaviorBase behaviorBase;
    //Which time delta we are using for this timer
    TimeDeltaType type = TimeDeltaType.TimeScaleDependent;
    public TimeDeltaType timeDeltaType { get { return type; } }
    public enum TimeDeltaType { TimeScaleDependent, TimeScaleIndependent }
    //Event signaling when some timer finished work and is ready to be released
    event Action<Timer> OnStopped;
    public event Action OnUpdate;
    public bool WasDestroyed { get; private set; }
    public bool DontDisposeOnComplete { get; set; }

    public float TimePassedTotal { get; private set; }
    public float TimePassed { get; private set; }
    public float TimeLeft { get; private set; }
    /// <summary>
    /// Not all timer have one fixed interval, this is the first "value" the timer will use, Countdown will use it as countdown time, Repeater will use it as
    /// main interval between repeats and so on
    /// </summary>
    public float MainInterval
    {
        get { return behaviorBase.MainInterval; }
        set { behaviorBase.MainInterval = value; }
    }
    /// <summary>
    /// Not all timers return this, how many cycles have passed
    /// 
    /// </summary>
    public float ElapsedCycles { get { return behaviorBase.ElapsedCycles; } }

    void Update(float REAL_DELTA, float TIMESCALE_DELTA)
    {
        //skip the ones that were completed, works with DontDisposeOnComplete flag
        if (behaviorBase.Completed)
            return;
        //first check for potential problems
        behaviorBase.CheckErrors();

        if (type == TimeDeltaType.TimeScaleDependent)
            behavior.Update(TIMESCALE_DELTA);
        else
            behavior.Update(REAL_DELTA);

        TimePassedTotal = behaviorBase.TotalTimeActive;
        TimePassed = behaviorBase.TimePassed;
        TimeLeft = MainInterval - TimePassed;

        if (OnUpdate != null)
            OnUpdate(); 
        //Notify pool if we completed work
        if (behaviorBase.Completed)
        {
            if (DontDisposeOnComplete)
                return;
            OnStopped(this);
            return;
        }
    }


    //again shortening common actions
    Timer registerBehavior(ITimerBehavior behav)
    {
        behavior = behav;
        behaviorBase = (TimerBehaviorBase)behavior;
        behaviorBase.timer = this;
        return this;
    }

    Timer SetBehavior<T>() where T : TimerBehaviorBase, new()
    {
        Type t = typeof(T);
        ITimerBehavior behav = null;
        if (TimerManager.behaviors.ContainsKey(t))
        {
            List<ITimerBehavior> list = TimerManager.behaviors[t];
            if (list.Count > 0)
            {
                behav = list[0];
                list.Remove(behav);
            }
        }
        if (behav == null)
            behav = (ITimerBehavior)new T();

        registerBehavior(behav);
        return this;
    }

    public void Destroy()
    {
        TimerManager.ReleaseTimer(this);
    }


    /// <summary>
    /// Do we use Time.deltaTime OR Time.unscaledTimeDelta
    /// unscaled means not affected by Time scale, by default all timers ARE affected by timescale
    /// </summary>
    /// <param name="scaled"></param>
    /// <returns></returns>
    public Timer SetIgnoreTimeScale(bool ignoreTimeScale)
    {
        type = ignoreTimeScale ? TimeDeltaType.TimeScaleIndependent : TimeDeltaType.TimeScaleDependent;
        return this;
    }


    public Timer SetCallbacks(Action c1)
    {
        behaviorBase.SetCallbacks(c1, null, null, null);
        return this;
    }

    public Timer SetCallbacks(Action c1, Action c2)
    {
        behaviorBase.SetCallbacks(c1, c2, null, null);
        return this;
    }

    public Timer SetCallbacks(Action c1, Action c2, Action c3)
    {
        behaviorBase.SetCallbacks(c1, c2, c3, null);
        return this;
    }

    public Timer SetCallbacks(Action c1, Action c2, Action c3, Action c4)
    {
        behaviorBase.SetCallbacks(c1, c2, c3, c4);
        return this;
    }

    public Timer Reset()
    {
        behaviorBase.ResetTime();
        return this;
    }

    public Timer Pause()
    {
        Paused = true;
        return this;
    }

    public Timer Unpause()
    {
        Paused = false;
        return this;
    }

    /// <summary>
    /// We use this class to create timer types. We use its variables in concrete implementations, as per implementation.
    /// 
    /// </summary>
    public class TimerBehaviorBase
    {
        
        public bool Completed { get; protected set; }
        public Timer timer;

        //How long the timer is UP (with all the pauses, resets, basically how long it lives since taken from pool)
        public float TotalTimeActive { get; protected set; }
        //Implementers can use these variables as they need
        public float TimePassed { get; protected set; }
        //Internally used floats
        protected float f1, f2, f3, f4;
        //callbacks
        protected Action c1, c2, c3, c4;
        //callbacks with parameters
        protected Action<object[]> paramC1, paramC2;
        //parameters array
        protected object[] parameters;
        protected bool hasParameters;
        /// <summary>
        /// optional depends on implementation
        /// </summary>
        public int ElapsedCycles { get; protected set; }
        //use this to expose your "main" driving value of the timer
        public float MainInterval { get { return f1; } set { f1 = value; } }
        public TimerBehaviorBase ResetEntity()
        {
            ElapsedCycles = 0;
            TimePassed = 0f;
            TotalTimeActive = 0f;
            MainInterval = 0f;
            Completed = false;

            parameters = null;
            c1 = c2 = c3 = c4 = null;
            hasParameters = false;
            paramC1 = paramC2 = null;
            f1 = f2 = f3 = f4 = 0f;
            timer = null;
            

            return this;
        }
        public void CheckErrors()
        {
            if (!hasParameters && c1 == null && c2 == null && c3 == null && c4 == null)
            {
                Debug.LogError("Timer cant have zero callbacks, such timer is useless. Make sure to add a callback before the timer" +
                     "starts execution");
                Debug.Break();
            }
            if (hasParameters && paramC1 == null && paramC2 == null)
            {
                Debug.LogError("PARAMETER Timer cant have zero parameter callbacks, such timer is useless. Make sure to add a callback before the timer" +
                     "starts execution");
                Debug.Break();
            }
        }

        public TimerBehaviorBase SetCallbacks(Action one, Action two, Action three, Action four)
        {
            c1 = one;
            c2 = two;
            c3 = three;
            c4 = four;
            return this;
        }

        public TimerBehaviorBase SetCallbacksWithParameters(Action<object[]> pc1, Action<object[]> pc2, object[] p)
        {
            paramC1 = pc1;
            paramC2 = pc2;
            parameters = p;
            hasParameters = true;
            return this;
        }

        public TimerBehaviorBase SetFloats(float one, float two, float three, float four)
        {
            //Typically in most scenarios first float will be our main "driving" value for the timer.
            MainInterval = one;

            f1 = one;
            f2 = two;
            f3 = three;
            f4 = four;
            return this;
        }

        public TimerBehaviorBase ResetTime()
        {
            TimePassed = 0f;
            Completed = false;
            return this;
        }
    }

    /// <summary>
    /// This class handles updating and disposing of timers
    /// </summary>
    public static class TimerManager
    {
        /// <summary>
        /// Behavior objects pool 
        /// </summary>
        public static Dictionary<Type, List<ITimerBehavior>> behaviors = new Dictionary<Type, List<ITimerBehavior>>();
        public static List<Timer> workingTimers = new List<Timer>();
        public static List<Timer> freeTimers = new List<Timer>();
        public static List<Timer> toRemove = new List<Timer>();

        public static void POLL_TIMER_DATA(TimerHelper.TimerHelperData data)
        {
            data.FreeTimers = freeTimers.Count;
            data.WorkingTimers = workingTimers.Count;
            data.AllTimers = data.FreeTimers + data.WorkingTimers;
        }

        public static void UpdateAllTimers()
        {
            int c = workingTimers.Count;
            //update all timers
            for (int i = 0; i < c; i++)
            {
                if (!workingTimers[i].WasDestroyed && !workingTimers[i].Paused)
                    workingTimers[i].Update(Time.unscaledDeltaTime, Time.deltaTime);
            }

            c = toRemove.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    workingTimers.Remove(toRemove[i]);
                }
                toRemove.Clear();
            }
        }

        //attempt to get used timer from pool or make new
        public static Timer getTimer()
        {
            Timer t = null;
            if (freeTimers.Count == 0)
            {
                t = newTimer();
            }
            else
            {
                t = freeTimers[0];
                freeTimers.Remove(t);
            }
            //Just to make sure our Helper appears when we need it
            if (TimerHelper.instance == null)
            { }
            registerTimer(t);
            return t;
        }
        // avoiding boilerplate
        public static Timer newTimer()
        {
            Timer t = new Timer();
            //easy way to get notified of which timer was stopped
            t.OnStopped += ReleaseTimer;
            return t;
        }

        public static void registerTimer(Timer timer)
        {
            timer.WasDestroyed = false;
            workingTimers.Add(timer);
        }
        //We do this at the end of timer lifecycle or when we destroy it
        public static void ReleaseTimer(Timer timer)
        {
            if (timer.WasDestroyed)
                return;
            freeTimers.Add(timer);

            //we use type as key to cache our behaviors
            Type btype = timer.behavior.GetType();
            if (!behaviors.ContainsKey(btype))
                behaviors.Add(btype, new List<ITimerBehavior>());
            behaviors[btype].Add(timer.behavior);

            timer.Paused = false;
            timer.behavior = null;
            timer.behaviorBase.ResetEntity();
            timer.WasDestroyed = true;
            toRemove.Add(timer);
        }
    }

    public static void KillAll()
    {
        int c = TimerManager.workingTimers.Count;
        for (int i = 0; i < c; i++)
        {
            TimerManager.workingTimers[i].Destroy();
        }
    }

}
