using System.Collections.Generic;
using System.Windows;

namespace LogoFX.Client.Theming
{
    internal abstract class ThemeTree : ThemeBase, IThemeTree
    {
        #region Fields

        private readonly SortedList<int, ITheme> _nodes =
            new SortedList<int, ITheme>();

        private ITheme _activeNode;

        #endregion

        #region Constructors

        protected ThemeTree(string name, int order) 
            : base(name, order)
        {
        }

        #endregion

        #region Internal

        internal void AddNode(ITheme node)
        {
            if (_activeNode == null)
            {
                _activeNode = node;
            }

            _nodes.Add(node.Order, node);
        }

        #endregion

        #region Overrides

        protected override ResourceDictionary[] LoadResoucesInternal(HashSet<string> dics)
        {
            var result = new List<ResourceDictionary>();

            if (ActiveNode != null)
            {
                result.AddRange(ActiveNode.LoadResources());
            }

            return result.ToArray();
        }

        #endregion

        #region IThemeTree

        public IEnumerable<ITheme> Nodes
        {
            get { return _nodes.Values; }
        }

        public ITheme ActiveNode
        {
            get { return _activeNode; }
            set
            {
                if (_activeNode == value)
                {
                    return;
                }

                _activeNode = value;
                RaiseUpdated();
            }
        }

        #endregion
    }
}