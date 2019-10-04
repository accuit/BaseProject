using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using AccuIT.CommonLayer.Aspects.Logging;

namespace AccuIT.CommonLayer.Aspects.Security
{
    /// <summary>
    /// Class to provide the annotation or config file driven validation feature over the data entities and objects
    /// To implement the annotation driven validation enterprise library validation block is used in application
    /// </summary>
    public static class ValidationEngine
    {
        private static ValidatorFactory _validationManager;

        /// <summary>
        /// Method to initialize validation services
        /// </summary>
        private static void InitializeValidationService()
        {
            try
            {
                _validationManager = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            }
            catch (Exception ex)
            {
                LogTraceEngine.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// Method to get validation results
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="itemInstance">item instance to validate</param>
        /// <param name="ruleSet">rule set name</param>
        /// <returns>returns validation result</returns>
        public static ValidationResults GetValidationResult<T>(T itemInstance, string ruleSet)
        {
            var validator = _validationManager.CreateValidator<T>(ruleSet);
            return validator.Validate(itemInstance);
        }

        /// <summary>
        /// Overload method to returns validation results from an entity
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="itemInstance">entity instance</param>
        /// <returns>returns validation result</returns>
        public static ValidationResults GetValidationResult<T>(T itemInstance)
        {
            var validator = _validationManager.CreateValidator<T>();
            return validator.Validate(itemInstance);
        }

        /// <summary>
        /// Method to validate Data contract instances using enterprise library validation manager 
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <typeparam name="K">Generic Type K</typeparam>
        /// <param name="firstItemInstance">first item instance</param>
        /// <param name="secondItemInstance">second item instance</param>
        /// <param name="ruleSets">rule set names</param>
        /// <returns>returns validation results</returns>
        public static ValidationResults GetValidationResult<T, K>(T firstItemInstance, K secondItemInstance, Tuple<string, string> ruleSets)
        {
            var firstItemValidator = _validationManager.CreateValidator<T>(ruleSets.Item1);
            var secondItemValidator = _validationManager.CreateValidator<K>(ruleSets.Item2);
            ValidationResults results = firstItemValidator.Validate(firstItemInstance);
            if (results == null)
                results = new ValidationResults();
            results.AddAllResults(secondItemValidator.Validate(secondItemInstance));
            return results;
        }

        /// <summary>
        /// Method to get validation results for two item instances using attribute driven validator
        /// </summary>
        /// <typeparam name="T">Generic Type of Item 1</typeparam>
        /// <typeparam name="K">Generic Type of Item 2</typeparam>
        /// <param name="firstItemInstance">item1</param>
        /// <param name="secondItemInstance">item2</param>
        /// <returns>returns validation result</returns>
        public static ValidationResults GetValidationResult<T, K>(T firstItemInstance, K secondItemInstance)
        {
            var firstItemValidator = _validationManager.CreateValidator<T>();
            var secondItemValidator = _validationManager.CreateValidator<K>();
            ValidationResults results = firstItemValidator.Validate(firstItemInstance);
            if (results == null)
                results = new ValidationResults();
            results.AddAllResults(secondItemValidator.Validate(secondItemInstance));
            return results;
        }

        /// <summary>
        /// Method to get validation results for collection of attribute driven class
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="items">collection of item</param>
        /// <returns>returns validation result</returns>
        public static ValidationResults GetValidationResult<T>(List<T> items) where T : class
        {
            Validator collectionValidator = new ObjectCollectionValidator(typeof(T));
            return collectionValidator.Validate(items);
        }

        /// <summary>
        /// Method to build Error collection in response
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="errors">error list</param>
        ///<returns>returns array of validation failed messages</returns>
        public static string[] BuildValidationErrors<T>(ValidationResults errors)
        {
            string[] failedValidations = new string[errors.Count];
            int index = 0;
            foreach (var result in errors)
            {
                failedValidations[index] = result.Message;
                index++;
            }
            return failedValidations;
        }

        /// <summary>
        /// Method to get validation errors
        /// </summary>
        /// <param name="errors">error list</param>
        /// <returns>returns error list</returns>
        public static string[] GetValidationErrors(ValidationResults errors)
        {
            string[] errorList = new string[errors.Count];
            int index = 0;
            foreach (var result in errors)
            {
                errorList[index] = result.Message;
                index++;
            }
            return errorList;
        }
    }
}
