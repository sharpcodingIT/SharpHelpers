// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpCoding.SharpHelpers;
using System;
using System.Collections.Generic;

namespace SharpHelpers.UnitTest.Dictionary
{
    [TestClass]
    public class DictionaryTest
    {
        [TestMethod]
        public void AddFormat_ShouldAddFormattedStringToDictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            dictionary.AddFormat(1, "Hello, {0}!", "World");

            // Assert
            Assert.AreEqual("Hello, World!", dictionary[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFormat_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            dictionary.AddFormat(1, "Hello, {0}!", "World");

            // Assert - [ExpectedException] handles the assertion
        }

        [TestMethod]
        public void RemoveAll_ShouldRemoveItemsBasedOnCondition()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Apple" },
                { 2, "Banana" },
                { 3, "Avocado" }
            };

            // Act
            dictionary.RemoveAll(kvp => kvp.Value.StartsWith("A"));

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveAll_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            dictionary.RemoveAll(kvp => kvp.Value.StartsWith("A"));

            // Assert - [ExpectedException] handles the assertion
        }

        [TestMethod]
        public void GetOrCreate_ShouldCreateAndReturnNewValue_WhenKeyDoesNotExist()
        {
            // Arrange
            var dictionary = new Dictionary<int, List<string>>();

            // Act
            var result = dictionary.GetOrCreate(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsTrue(dictionary.ContainsKey(1));
        }

        [TestMethod]
        public void AddOrUpdate_ShouldAddNewItem_WhenKeyDoesNotExist()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            dictionary.AddOrUpdate(1, "NewValue");

            // Assert
            Assert.AreEqual("NewValue", dictionary[1]);
        }

        [TestMethod]
        public void AddOrUpdate_ShouldUpdateExistingItem_WhenKeyExists()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "OldValue" }
            };

            // Act
            dictionary.AddOrUpdate(1, "UpdatedValue");

            // Assert
            Assert.AreEqual("UpdatedValue", dictionary[1]);
        }

        [TestMethod]
        public void RemoveIfExists_ShouldReturnTrueAndRemoveItem_WhenKeyExists()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value" }
            };

            // Act
            var result = dictionary.RemoveIfExists(1);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(dictionary.ContainsKey(1));
        }

        [TestMethod]
        public void RemoveIfExists_ShouldReturnFalse_WhenKeyDoesNotExist()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            var result = dictionary.RemoveIfExists(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Merge_ShouldMergeDictionariesAndUpdateValues()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value1" },
                { 2, "Value2" }
            };

            var otherDictionary = new Dictionary<int, string>
            {
                { 2, "UpdatedValue2" },
                { 3, "Value3" }
            };

            // Act
            dictionary.Merge(otherDictionary);

            // Assert
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual("UpdatedValue2", dictionary[2]);
            Assert.AreEqual("Value3", dictionary[3]);
        }

        [TestMethod]
        public void ToReadableString_ShouldReturnFormattedStringRepresentationOfDictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value1" },
                { 2, "Value2" }
            };

            // Act
            var result = dictionary.ToReadableString();

            // Assert
            Assert.AreEqual("{1: Value1, 2: Value2}", result);
        }
    }
}