namespace WebUser.shared.extentions;

using WebUser.Data;

public static class ManyToManyEntitiesUpdater
{
    /// <summary>
    /// Update many to many relation's binding table
    /// </summary>
    /// <typeparam name="TEntity">main entity</typeparam>
    /// <typeparam name="TRelated">related entity</typeparam>
    /// <typeparam name="TRelation">binding entity</typeparam>
    /// <param name="dbcontext">DB</param>
    /// <param name="baseEntity">main entity</param>
    /// <param name="relationEntities">binding entities</param>
    /// <param name="newIds">list of new related ids</param>
    /// <param name="relationFactory">function that sets rules of binding objects creation</param>
    /// <param name="relatedEntityFinder">function that finds related objects by its id</param>
    /// <returns></returns>
    public static async Task UpateManyToManyRelationsAsync<TEntity, TRelated, TRelation>(
        DB_Context dbcontext,
        TEntity baseEntity,
        ICollection<TRelation> relationEntities,
        ICollection<int> newIds,
        Func<TEntity, TRelated, TRelation> relationFactory,
        //Func<int, Task<TRelated>> relatedEntityFinder
        Func<ICollection<int>, Task<List<TRelated>>> relatedEntityFinder
    )
        where TEntity : class
        where TRelated : class
        where TRelation : class
    {
        if (newIds == null || !newIds.Any())
        {
            return;
        }

        var relatedObjects = await relatedEntityFinder(newIds);

        if (!relatedObjects.Any())
        {
            return;
        }

        var relationSet = dbcontext.Set<TRelation>();
        relationEntities.Clear(); //clear previous bindings

        var newRelations = relatedObjects.Select(relatedObject => relationFactory(baseEntity, relatedObject)).ToList(); //create new bindings
        foreach (var relation in newRelations)
        {
            relationEntities.Add(relation);
            relationSet.Add(relation);
        }
    }
}
