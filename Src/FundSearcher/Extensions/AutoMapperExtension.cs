using System;
using AutoMapper;

namespace FundSearcher.Extensions
{
    static class AutoMapperExtension
    {
        private static IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        }).CreateMapper();

        public static TTo Map<TFrom, TTo>(this TFrom from, Action<TTo> action = null)
        {
            var result = mapper.Map<TTo>(from);
            action?.Invoke(result);
            return result;
        }

        public static TTo Map<TFrom, TTo>(this TFrom from, TTo to, Action<TTo> action = null)
        {
            var result = mapper.Map(from, to);
            action?.Invoke(result);
            return result;
        }
    }
}
