﻿using Xunit;
using KissTools.Tests.Mock;
using System.Text.Json;

namespace KissTools.Tests
{
    public class AutoMapperShould
    {
        [Fact]
        public void MapAttributeWithSameName()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { Name = "Saturn" };
            //Act
            AutoMapper.From(s).MapTo(t).Go();
            //Assert
            Assert.Equal("Saturn", t.Name);
        }

        [Fact]
        public void MapAttributeWithSameNameIgnoringCase()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { perimeter = 58.232 };
            //Act
            AutoMapper.From(s).MapTo(t, MapperOption.IGNORE_CASE).Go();
            //Assert
            Assert.Equal(58.232, t.Perimeter);
        }

        [Fact]
        public void MapAttributeWithSameNameIgnoringUnderscore()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { Distance_To_Sun = 9.53707032 };
            //Act
            AutoMapper.From(s).MapTo(t, MapperOption.IGNORE_UNDERSCORE).Go();
            //Assert
            Assert.Equal(9.53707032, t.DistanceToSun);
        }

        [Fact]
        public void MapAttributeWithSameNameIgnoringCaseAndUnderscore()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { Orbital_speed = 9672.4 };
            //Act
            AutoMapper.From(s).MapTo(t, MapperOption.IGNORE_UNDERSCORE | MapperOption.IGNORE_CASE).Go();
            //Assert
            Assert.Equal(9672.4, t.OrbitalSpeed);
        }

        [Fact]
        public void MapAttributeWithDifferentName()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { atmosfericPressure = 1013.14 };
            //Act
            AutoMapper.From(s).MapTo(t).Link(o => o.atmosfericPressure).InTo(o => o.Pressure).Go();
            //Assert
            Assert.Equal(1013.14, t.Pressure);
        }
        
        [Fact]
        public void MapAttributeWhithSameNameAndDifferentParseableType()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { HasMoons = "true" };
            //Act
            AutoMapper.From(s).MapTo(t, MapperOption.FORCE_TYPE).Go();
            //Assert
            Assert.True(t.HasMoons);
        }

        [Fact]
        public void MapAttributeWhithSameNameAndNullValue()
        {
            //Arrange
            TargetClass t = new TargetClass() { Name = "Mars" };
            SourceClass s = new SourceClass() { Name = null };
            //Act
            AutoMapper.From(s).MapTo(t).Go();
            //Assert
            Assert.Null(t.Name);
        }

        [Fact]
        public void MapAttributeWhithDifferentNameAndNullValue()
        {
            //Arrange
            TargetClass t = new TargetClass() { Name = "Mars" };
            SourceClass s = new SourceClass() { Alias = null };
            //Act
            AutoMapper.From(s).MapTo(t).Link(o => o.Alias).InTo(o => o.Name).Go();
            //Assert
            Assert.Null(t.Name);
        }

        [Fact]
        public void MapAttributeOfDynamicObjects()
        {
            //Arrange
            TargetClass t = new TargetClass();
            var s = new { Name = "Venus" };
            //Act
            AutoMapper.From(s).MapTo(t).Go();
            //Assert
            Assert.Equal("Venus", t.Name);
        }

        [Fact]
        public void MapAttributeOfJsonObject()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = JsonSerializer.Deserialize<SourceClass>("{\"Name\":\"Jupiter\"}");
            //Act
            AutoMapper.From(s).MapTo(t).Go();
            //Assert
            Assert.Equal("Jupiter", t.Name);
        }

        [Fact]
        public void MapAttributeAndIgnoreOther()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { Name = "Mercury", CodeName = "1BX17E" };
            //Act
            AutoMapper.From(s).MapTo(t).Ignoring(o => o.CodeName).Go();
            //Assert
            Assert.Equal(new[] { "Mercury", null }, new[] { t.Name, t.CodeName });
        }

        [Fact]
        public void MapAnAttributeMappedBefore()
        {
            //Arrange
            TargetClass t = new TargetClass();
            SourceClass s = new SourceClass() { Name = "Mercury" };
            //Act
            AutoMapper.MapBuild<SourceClass, TargetClass> build = AutoMapper.From(s).MapTo(t);
            s.Name = "Jupiter";
            build.Link(o => o.Name).InTo(o => o.Name).Go();
            //Assert
            Assert.Equal("Jupiter", t.Name);
        }

        [Fact]
        public void MapTwoTargets()
        {
            //Arrange
            SourceClass s = new SourceClass() { Name = "Mercury" };
            TargetClass t1 = new TargetClass();
            TargetClass t2 = new TargetClass();
            //Act
            AutoMapper.From(s).MapTo(t1).Go().MapTo(t2).Go();
            //Assert
            Assert.Equal(new[] { "Mercury", "Mercury" } , new[] { t1.Name, t2.Name });
        }

    }
}
