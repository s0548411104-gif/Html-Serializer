using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
        public List<string> Classes { get; set; } = new();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new();
        public override string ToString() =>
            $"<{Name}{(Id != null ? $" id='{Id}'" : "")}{(Classes.Any() ? $" class='{string.Join(" ", Classes)}'" : "")}>";
        public IEnumerable<HtmlElement> Descendants()
        {
            var stack = new Stack<HtmlElement>();
            var visited = new HashSet<HtmlElement>();
            stack.Push(this);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current))
                    continue; 

                yield return current;

                foreach (var child in current.Children)
                    stack.Push(child);
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this.Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public List<HtmlElement> Query(Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            QueryRecursive(this, selector, results);
            return results.ToList();
        }
        private void QueryRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            foreach (var desc in element.Children)
            {
                bool match = true;

                if (!string.IsNullOrEmpty(selector.TagName))
                    match &= desc.Name == selector.TagName;

                if (!string.IsNullOrEmpty(selector.Id))
                    match &= desc.Id == selector.Id;

                foreach (var cls in selector.Classes)
                    match &= desc.Classes.Contains(cls);

                if (match)
                {
                    if (selector.Child == null)
                    {
                        results.Add(desc);
                    }
                    else
                    {
                        QueryRecursive(desc, selector.Child, results);
                    }
                }
                QueryRecursive(desc, selector, results);
            }
        }
    }
}