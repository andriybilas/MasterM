using Litium.Common;
using Litium.Common.Lifecycle;
using Litium.Common.WorkUnit;
using Litium.Test.Common.Xunit.Base;
using Litium.Test.Common.Xunit.TestEntities;
using Xunit;

namespace Litium.Test.Common.Xunit.WorkUnit
{
	public class UnitOfWorkTest : ConversationalTestBase
	{
		[Fact]
		public void NestedUnitsCombinatedTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};
			var entity3 = new SimpleEntity
			              	{
			              		Name = "UnitTest3"
			              	};
			var entity4 = new SimpleEntity
			              	{
			              		Name = "UnitTest4"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);

				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					using (var unit3 = new UnitOfWork())
					{
						Repository.Data.Save(entity3);
						unit3.Commit();
					}
					using (var unit4 = new UnitOfWork(UnitOfWorkScopeType.New))
					{
						Repository.Data.Save(entity4);
						unit4.Commit();
					}
					Assert.Throws<UnitOfWorkException>(() => unit2.Rollback());
				}

				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			entity3 = Repository.Data.Get<SimpleEntity>(entity3.Id);
			entity4 = Repository.Data.Get<SimpleEntity>(entity4.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
			Assert.Null(entity3);
			Assert.NotNull(entity4);
		}

		[Fact]
		public void NestedUnitsWithInnerScopeRollbackAndCommitActionTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				UnitOfWork.Current.PostCommitActions.Add(() => { });
				UnitOfWork.Current.PostRollbackActions.Add(() => { });
				Repository.Data.Save(entity);
				Assert.Equal(1, UnitOfWork.Current.PostCommitActions.Count);
				Assert.Equal(1, UnitOfWork.Current.PostRollbackActions.Count);


				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					UnitOfWork.Current.PostCommitActions.Add(() => { });
					UnitOfWork.Current.PostRollbackActions.Add(() => { });
					Repository.Data.Save(entity2);
					unit2.Rollback();
				}

				Assert.Equal(1, UnitOfWork.Current.PostCommitActions.Count);
				Assert.Equal(1, UnitOfWork.Current.PostRollbackActions.Count);
				unit.Commit();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.NotNull(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithNewInnerScopeCommitAndRollbackTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					Repository.Data.Save(entity2);
					unit2.Commit();
				}
				unit.Rollback();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.NotNull(entity2);
		}

		[Fact]
		public void NestedUnitsWithNewInnerScopeCommitBothTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					Repository.Data.Save(entity2);
					unit2.Commit();
				}
				unit.Commit();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.NotNull(entity);
			Assert.NotNull(entity2);
		}

		[Fact]
		public void NestedUnitsWithNewInnerScopeRollbackAndCommitTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					Repository.Data.Save(entity2);
					unit2.Rollback();
				}
				unit.Commit();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.NotNull(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithNewInnerScopeRollbackBothTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					Repository.Data.Save(entity2);
					unit2.Rollback();
				}
				unit.Rollback();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeCommitAndRollbackTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					unit2.Commit();
				}
				unit.Rollback();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeCommitBothTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					unit2.Commit();
				}
				unit.Commit();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.NotNull(entity);
			Assert.NotNull(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeInnerDisposeTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				Assert.Throws<UnitOfWorkException>(() =>
				                                   	{
				                                   		using (new UnitOfWork())
				                                   		{
				                                   			Repository.Data.Save(entity2);
				                                   		}
				                                   	});
				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeOuterDisposeTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					unit2.Commit();
				}
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeRollbackAndCommitActionTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				int onCommitActionsCount;
				int onRollBackActionsCount;

				UnitOfWork.Current.PostCommitActions.Add(() => { });
				UnitOfWork.Current.PostRollbackActions.Add(() => { });
				Repository.Data.Save(entity);
				Assert.Equal(1, UnitOfWork.Current.PostCommitActions.Count);
				Assert.Equal(1, UnitOfWork.Current.PostRollbackActions.Count);

				using (var unit2 = new UnitOfWork())
				{
					UnitOfWork.Current.PostCommitActions.Add(() => { });
					UnitOfWork.Current.PostRollbackActions.Add(() => { });
					onCommitActionsCount = UnitOfWork.Current.PostCommitActions.Count;
					onRollBackActionsCount = UnitOfWork.Current.PostRollbackActions.Count;
					Repository.Data.Save(entity2);
					Assert.Throws<UnitOfWorkException>(() => unit2.Rollback());
				}

				Assert.Equal(2, onCommitActionsCount);
				Assert.Equal(2, onRollBackActionsCount);
				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeRollbackAndCommitTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					Assert.Throws<UnitOfWorkException>(() => unit2.Rollback());
				}
				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeRollbackBothTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					Assert.Throws<UnitOfWorkException>(() => unit2.Rollback());
				}
				unit.Rollback();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void UnitOfWorkCommitTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				unit.Commit();
			}

			Repository.Data.Cache.Clear(entity);
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.NotNull(entity);
		}

		[Fact]
		public void UnitOfWorkRollbackTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				unit.Rollback();
			}

			Repository.Data.Cache.Clear(entity);
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.Null(entity);
		}

		[Fact]
		public void UnitOfWorkCommitAfterRollBackTest()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);

				//Some external method
				UnitOfWork.Current.Rollback();

				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(entity);
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.Null(entity);
		}

		[Fact]
		public void UnitOfWorkRollBackAfterCommitTest()
		{
			var entity = new SimpleEntity
			{
				Name = "UnitTest"
			};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);

				//Some external method
				UnitOfWork.Current.Commit();

				Assert.Throws<UnitOfWorkException>(() => unit.Rollback());
			}

			Repository.Data.Cache.Clear(entity);
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.NotNull(entity);
		}

		[Fact]
		public void UnitOfWorkCommitAfterCommitTest()
		{
			var entity = new SimpleEntity
			{
				Name = "UnitTest"
			};
			var entity2 = new SimpleEntity
			{
				Name = "UnitTest2"
			};

			using (var unit = new UnitOfWork())
			{
				//Some external method
				Repository.Data.Save(entity);
				UnitOfWork.Current.Commit();

				Assert.Throws<UnitOfWorkException>(() => Repository.Data.Save(entity2));
				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(entity);
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.NotNull(entity);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity2);
		}

		[Fact]
		public void UnitOfWorkRollBackAfterRollbackTest()
		{
			var entity = new SimpleEntity
			{
				Name = "UnitTest"
			};
			var entity2 = new SimpleEntity
			{
				Name = "UnitTest2"
			};

			using (var unit = new UnitOfWork())
			{
				//Some external method
				Repository.Data.Save(entity);
				UnitOfWork.Current.Rollback();

				Assert.Throws<UnitOfWorkException>(() => Repository.Data.Save(entity2));
				unit.Rollback();
			}

			Repository.Data.Cache.Clear(entity);
			ConversationHelper.ReOpen();

			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.Null(entity);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithSameScopeInnerException()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork())
				{
					Repository.Data.Save(entity2);
					Assert.Throws<UnitOfWorkException>(() => unit2.Rollback());
						// will throw exception because this is not the owner of the UOW
				}

				// user will not come to this point and try to commit transaction
				// else it should throw exception
				Assert.Throws<UnitOfWorkException>(() => unit.Commit());
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();
			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.Null(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void NestedUnitsWithNewInnerScopeException()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					using (var unit3 = new UnitOfWork())
					{
						Repository.Data.Save(entity2);
						Assert.Throws<UnitOfWorkException>(() => unit3.Rollback());
							// will throw exception because this is not the owner of the UOW
					}
					// user will not come to this point and try to commit transaction
					// else it should throw exception
					Assert.Throws<UnitOfWorkException>(() => unit2.Commit());
				}

				// user can commit the outer UOW if unit2-UOW is inside an try/catch
				unit.Commit();
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();
			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			entity2 = Repository.Data.Get<SimpleEntity>(entity2.Id);
			Assert.NotNull(entity);
			Assert.Null(entity2);
		}

		[Fact]
		public void ScopeWithoutException()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				unit.Rollback(); // will not throw exception
			}

			Repository.Data.Cache.Clear(typeof (SimpleEntity));
			ConversationHelper.ReOpen();
			entity = Repository.Data.Get<SimpleEntity>(entity.Id);
			Assert.Null(entity);
		}

		[Fact]
		public void NestedUnitsWithInnerScopeWithoutException()
		{
			var entity = new SimpleEntity
			             	{
			             		Name = "UnitTest"
			             	};
			var entity2 = new SimpleEntity
			              	{
			              		Name = "UnitTest2"
			              	};

			using (var unit = new UnitOfWork())
			{
				Repository.Data.Save(entity);
				using (var unit2 = new UnitOfWork(UnitOfWorkScopeType.New))
				{
					Repository.Data.Save(entity2);
					unit2.Rollback(); // will not throw exception because this is own transaction
				}
				// user can commit the outer UOW
				unit.Commit();
			}
		}
	}
}