using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MultiTypeBinderExpr.Tests
{
    public enum Key
    {
        Name,
        RandomKey
    }

    public class EntityA
    {
        public string Name1 { get; set; }
    }

    public class EntityB
    {
        public string Name2 { get; set; }
    }

    public class MultiTypeBinderTest
    {
        [Fact]
        public void Test__Get()
        {
            // Arrange
            var a = new EntityA {Name1 = "A"};
            var b = new EntityB {Name2 = "B"};

            var multiTypeItems = new MultiTypeBinderBuilder<Key>()
                .WithType<EntityA>(opt1 => opt1
                    .WithProperty(x => x.Name1, Key.Name)
                    .FinalizeType())
                .WithType<EntityB>(opt1 => opt1
                    .WithProperty(x => x.Name2, Key.Name)
                    .FinalizeType())
                .Build()
                .Map(new List<object> {a, b});

            // Act
            var v1 = multiTypeItems.First()[Key.Name];
            var v2 = multiTypeItems.Last()[Key.Name];

            // Assert
            Assert.Equal(2, multiTypeItems.Count);
            Assert.Equal("A", v1);
            Assert.Equal("B", v2);
        }

        [Fact]
        public void Test__Set()
        {
            // Arrange
            var a = new EntityA {Name1 = "A"};
            var b = new EntityB {Name2 = "B"};

            var multiTypeItems = new MultiTypeBinderBuilder<Key>()
                .WithType<EntityA>(opt1 => opt1
                    .WithProperty(x => x.Name1, Key.Name)
                    .FinalizeType())
                .WithType<EntityB>(opt1 => opt1
                    .WithProperty(x => x.Name2, Key.Name)
                    .FinalizeType())
                .Build()
                .Map(new List<object> {a, b});

            // Act
            multiTypeItems.First()[Key.Name] = "updated A";
            multiTypeItems.Last()[Key.Name] = "updated B";

            var v1 = multiTypeItems.First()[Key.Name];
            var v2 = multiTypeItems.Last()[Key.Name];

            // Assert
            Assert.Equal(2, multiTypeItems.Count);
            Assert.Equal("updated A", v1);
            Assert.Equal("updated B", v2);
        }

        [Fact]
        public void Test__Get_Fail()
        {
            // Arrange
            var source = new EntityB {Name2 = "A"};
            
            var multiTypeItems = new MultiTypeBinderBuilder<Key>()
                .WithType<EntityA>(opt1 => opt1
                    .WithProperty(x => x.Name1, Key.Name)
                    .FinalizeType())
                .WithType<EntityB>(opt1 => opt1
                    .WithProperty(x => x.Name2, Key.Name)
                    .FinalizeType())
                .Build()
                .Map(new List<object> {source});

            // Act, Assert
            Assert.Throws<Exception>(() => multiTypeItems.First()[Key.RandomKey]);
        }

        [Fact]
        public void Test__Set_Fail()
        {
            // Arrange
            var source = new EntityA {Name1 = "A"};

            var multiTypeItems = new MultiTypeBinderBuilder<Key>()
                .WithType<EntityA>(opt1 => opt1
                    .WithProperty(x => x.Name1, Key.Name)
                    .FinalizeType())
                .WithType<EntityB>(opt1 => opt1
                    .WithProperty(x => x.Name2, Key.Name)
                    .FinalizeType())
                .Build()
                .Map(new List<object> {source});

            // Act, Assert
            Assert.Throws<InvalidCastException>(() => multiTypeItems.First()[Key.Name] = 123);
        }
    }
}