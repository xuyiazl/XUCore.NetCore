namespace XUCore.Collection
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:51:41
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    public class TreeNode<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Value of the node</param>
        /// <param name="Parent">Parent node</param>
        /// <param name="Left">Left node</param>
        /// <param name="Right">Right node</param>
        public TreeNode(T Value = default(T), TreeNode<T> Parent = null, TreeNode<T> Left = null, TreeNode<T> Right = null)
        {
            this.Value = Value;
            this.Right = Right;
            this.Left = Left;
            this.Parent = Parent;
        }

        /// <summary>
        /// Value of the node
        /// </summary>
        public virtual T Value { get; set; }

        /// <summary>
        /// Parent node
        /// </summary>
        public virtual TreeNode<T> Parent { get; set; }

        /// <summary>
        /// Left node
        /// </summary>
        public virtual TreeNode<T> Left { get; set; }

        /// <summary>
        /// Right node
        /// </summary>
        public virtual TreeNode<T> Right { get; set; }

        /// <summary>
        /// Is this the root
        /// </summary>
        public virtual bool IsRoot { get { return Parent == null; } }

        /// <summary>
        /// Is this a leaf
        /// </summary>
        public virtual bool IsLeaf { get { return Left == null && Right == null; } }

        internal bool Visited { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}