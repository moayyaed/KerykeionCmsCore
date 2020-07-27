using System.Collections.Generic;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class KerykeionDbResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static KerykeionDbResult Success(object entity = null) => ReturnSuccesfull(entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static KerykeionDbResult Fail(params KerykeionDbError[] errors) => AddResultErrors(errors);
        /// <summary>
        /// 
        /// </summary>
        public bool Successfull { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<KerykeionDbError> Errors { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Entity { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="successfull"></param>
        /// <param name="entity"></param>
        /// <param name="errors"></param>
        public KerykeionDbResult(bool successfull, object entity = null, params KerykeionDbError[] errors)
        {
            Successfull = successfull;
            Errors = errors;
            Entity = entity;
        }
        private static KerykeionDbResult AddResultErrors(KerykeionDbError[] errors)
        {
            if (errors.Length > 0)
            {
                return new KerykeionDbResult(false, errors: errors);
            }
            return new KerykeionDbResult(false);
        }

        private static KerykeionDbResult ReturnSuccesfull(object entity = null)
        {
            if (entity == null)
            {
                return new KerykeionDbResult(true);
            }
            return new KerykeionDbResult(true, entity);
        }
    }
}
