namespace BehaviorTree
{
    public enum InnerNode
    {
        ///<summary>
        /// Priority Sector.
        ///</summary>
        Priority,

        ///<summary>
        /// Sequence Sector.
        ///</summary>
        Sequence,

        ///<summary>
        /// Loop Sector.
        ///</summary>
        Loops,

        ///<summary>
        /// Random Sector.
        ///</summary>
        Random,

        ///<summary>
        /// Concurrent Sector.
        ///</summary>
        Concurrent,
    }

    public enum ActionState
    {
        ///<summary>
        /// Ready to run
        ///</summary>
        Ready,

        Running,

        ///<summary>
        /// Success completion
        ///</summary>
        Success,

        ///<summary>
        /// Fail without side effects
        ///</summary>
        Fail,

        ///<summary>
        /// Error with side effects
        ///</summary>
        Error
    }

    public enum Action
    {
        ///<summary>
        /// Called immediatly during the traversal
        ///</summary>
        Immediate,

        ///<summary>
        /// Callected the call request and batch run them after traversal
        ///</summary>
        Defferred,

        ///<summary>
        /// Run without explicit invocation.
        /// Always accessable during the traversal
        ///</summary>
        Persistent
    }
}