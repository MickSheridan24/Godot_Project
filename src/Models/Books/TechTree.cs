
using System;
using System.Collections.Generic;

public class TechTree
{
    public TechTreeNode Root { get; set; }


    public IEnumerable<TechTreeNode> AllTechs() => traverseTechs(Root, new List<TechTreeNode>(), (n) => true);


    public IEnumerable<TechTreeNode> AllUnlocked() => findUnlocked(Root, new List<TechTreeNode>());

    public IEnumerable<TechTreeNode> AllUnlockable() => findUnlockable(Root, new List<TechTreeNode>());


    private IEnumerable<TechTreeNode> traverseTechs(TechTreeNode curr, List<TechTreeNode> ret, Func<TechTreeNode, bool> cb)
    {
        if (cb(curr))
        {
            ret.Add(curr);
        }
        if (curr.Right != null)
        {
            ret.AddRange(traverseTechs(curr.Right, ret, cb));
        }
        if (curr.Left != null)
        {
            ret.AddRange(traverseTechs(curr.Left, ret, cb));
        }
        return ret;
    }


    private IEnumerable<TechTreeNode> findUnlocked(TechTreeNode curr, List<TechTreeNode> ret)
    {
        if (curr.Tech.Unlocked)
        {
            ret.Add(curr);
            if (curr.Right != null)
            {
                ret.AddRange(findUnlocked(curr.Right, ret));
            }
            if (curr.Left != null)
            {
                ret.AddRange(findUnlocked(curr.Left, ret));
            }
        }
        return ret;
    }


    private IEnumerable<TechTreeNode> findUnlockable(TechTreeNode curr, List<TechTreeNode> ret)
    {
        if (curr.Tech.Unlocked)
        {
            if (curr.Right != null)
            {
                if (curr.Right.Tech.Unlocked)
                {
                    ret.AddRange(findUnlockable(curr.Right, ret));
                }
                else
                {
                    ret.Add(curr.Right);
                }
            }

            if (curr.Left != null)
            {
                if (curr.Left.Tech.Unlocked)
                {
                    ret.AddRange(findUnlockable(curr.Left, ret));
                }
                else
                {
                    ret.Add(curr.Left);
                }
            }
        }
        return ret;
    }
}
