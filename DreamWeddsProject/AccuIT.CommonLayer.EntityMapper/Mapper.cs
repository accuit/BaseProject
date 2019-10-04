using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AccuIT.CommonLayer.EntityMapper
{
    /// <summary>
    /// Mapper class to provide automatic mapping action on the basis of registration types of objects defined
    /// Before calling the Map function it must be ensured that objects are registered in AutoMapper's memory container
    /// </summary>
    public class Mapper : IMapper
    {
        /// <summary>
        /// Method to map object from source into target
        /// </summary>
        /// <typeparam name="T">source generic type</typeparam>
        /// <typeparam name="K">target object generic type</typeparam>
        /// <param name="source">source object</param>
        /// <param name="target">target object</param>
        public void Map<T, K>(T source, K target)
        {
            AutoMapper.Mapper.Map(source, target);
        }

        /// <summary>
        /// Method to create register types for auto mapper mapping
        /// </summary>
        /// <typeparam name="T">source generic type</typeparam>
        /// <typeparam name="K">target object generic type</typeparam>
        public void CreateMap<T, K>()
        {
            AutoMapper.Mapper.CreateMap<T, K>();
        }
        ///// <summary>
        ///// Method to create register types for auto mapper mapping
        ///// </summary>
        ///// <typeparam name="T">source generic type</typeparam>
        ///// <typeparam name="K">target object generic type</typeparam>
        //public void CreateMapLeave<T, K>(string sourceProp, string destinationProp)
        //{
        //    AutoMapper.Mapper.CreateMap<T, K>().ForMember(;
        //}
        /// <summary>
        /// Method to create register types for auto mapper mapping
        /// </summary>
        /// <typeparam name="T">source generic type</typeparam>
        /// <typeparam name="K">target object generic type</typeparam>
        public void CreateMapLeave<T, K>(string sourceProp,string destinationProp)
        {
            AutoMapper.Mapper.CreateMap<T, K>().ForMember(x=>x.GetType().GetProperty(sourceProp),opt=>opt.MapFrom(src=>src.GetType().GetProperty(destinationProp)));
        }
        //public IMappingExpression<T, K> ForMember(Expression<Func<TDestination, object>> destinationMember, Action<IMemberConfigurationExpression<TSource>> memberOptions)
        //{

        //}

        //Expression<Func<TDestination, object>> destinationMember, Action<IMemberConfigurationExpression<TSource>> memberOptions
    }
}
