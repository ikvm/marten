﻿using System;
using System.Linq;
using Marten.Testing.Documents;
using Marten.Testing.Harness;
using Shouldly;
using Xunit;

namespace Marten.Testing.CoreFunctionality
{
    public class DocumentSession_change_set_tracking_Tests : IntegrationContext
    {
        [Fact]
        public void categorize_changes_inserts_and_deletions()
        {
            var logger = new RecordingLogger();
            theSession.Logger = logger;

            var target1 = new Target();
            var target2 = new Target();
            var target3 = new Target();

            var newDoc1 = new Target {Id = Guid.Empty};
            var newDoc2 = new Target {Id = Guid.Empty};

            theSession.Store(target1, target2, target3, newDoc1, newDoc2);

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            theSession.Delete<Target>(id1);
            theSession.Delete<Target>(id2);

            theSession.PendingChanges.UpdatesFor<Target>()
                .ShouldHaveTheSameElementsAs(target1, target2, target3);

            theSession.PendingChanges.InsertsFor<Target>()
                .ShouldHaveTheSameElementsAs(newDoc1, newDoc2);

            throw new NotImplementedException();
            // theSession.PendingChanges.DeletionsFor<Target>().OfType<DeleteById>().Select(x => x.Id)
            //     .ShouldHaveTheSameElementsAs(id1, id2);

            SpecificationExtensions.ShouldBeNull(logger.LastCommit);
            theSession.SaveChanges();

            // Everything should be cleared out
            theSession.PendingChanges.Updates().Any().ShouldBeFalse();
            theSession.PendingChanges.Inserts().Any().ShouldBeFalse();
            theSession.PendingChanges.Deletions().Any().ShouldBeFalse();

            logger.LastCommit.Updated.ShouldHaveTheSameElementsAs(target1, target2, target3);
            logger.LastCommit.Inserted.ShouldHaveTheSameElementsAs(newDoc1, newDoc2);

            throw new NotImplementedException();
            //logger.LastCommit.Deleted.OfType<DeleteById>().Select(x => x.Id).ShouldHaveTheSameElementsAs(id1, id2);

            theSession.Store(new Target());
            theSession.SaveChanges();

            logger.Commits.Count().ShouldBe(2);
            logger.LastCommit.Updated.Count().ShouldBe(1);
        }

        public DocumentSession_change_set_tracking_Tests(DefaultStoreFixture fixture) : base(fixture)
        {
        }
    }
}
