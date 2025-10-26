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

        public static TTo Map<TFrom, TTo>(this TFrom from, int index, Action<TTo, int> action = null)
        {
            var result = mapper.Map<TTo>(from);
            action?.Invoke(result, index);
            return result;
        }

        public static TTo Map<TFrom, TTo>(this TFrom from, TTo to, Action<TTo> action = null)
        {
            var result = mapper.Map(from, to);
            action?.Invoke(result);
            return result;
        }

        public static TTo[] Map<TFrom, TTo>(this TFrom[] from, Action<TTo> action = null)
        {
            var result = new TTo[from.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = from[i].Map(action);
            }
            return result;
        }

        public static TTo[] Map<TFrom, TTo>(this TFrom[] from, Action<TTo, int> action = null)
        {
            var result = new TTo[from.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = from[i].Map(i, action);
            }
            return result;
        }
    }
}
