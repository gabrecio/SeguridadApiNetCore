using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SEG.Repositories
{
    

    public static class EFCoreExtensions
    {
        public static void UpdateLinks<TLink, TFromId, TToId>(this DbSet<TLink> dbSet,
            Expression<Func<TLink, TFromId>> fromIdProperty, TFromId fromId,
            Expression<Func<TLink, TToId>> toIdProperty, IEnumerable<TToId> toIds)
            where TLink : class, new()
        {
            // link => link.FromId == fromId
            Expression<Func<TFromId>> fromIdVar = () => fromId;
            var filter = Expression.Lambda<Func<TLink, bool>>(
                Expression.Equal(fromIdProperty.Body, fromIdVar.Body),
                fromIdProperty.Parameters);
            var existingLinks = dbSet.AsTracking().Where(filter);

            var toIdSet = new HashSet<TToId>(toIds);
            if (toIdSet.Count == 0)
            {
                //The new set is empty - delete all existing links 
                dbSet.RemoveRange(existingLinks);
                return;
            }

            // Delete the existing links which do not exist in the new set
            var toIdSelector = toIdProperty.Compile();
            foreach (var existingLink in existingLinks)
            {
                if (!toIdSet.Remove(toIdSelector(existingLink)))
                    dbSet.Remove(existingLink);
            }

            // Create new links for the remaining items in the new set
            if (toIdSet.Count == 0) return;
            // toId => new TLink { FromId = fromId, ToId = toId }
            var toIdParam = Expression.Parameter(typeof(TToId), "toId");
            var createLink = Expression.Lambda<Func<TToId, TLink>>(
                Expression.MemberInit(
                    Expression.New(typeof(TLink)),
                    Expression.Bind(((MemberExpression)fromIdProperty.Body).Member, fromIdVar.Body),
                    Expression.Bind(((MemberExpression)toIdProperty.Body).Member, toIdParam)),
                toIdParam);
            dbSet.AddRange(toIdSet.Select(createLink.Compile()));
        }
    }
}
