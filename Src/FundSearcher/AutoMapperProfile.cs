using AutoMapper;
using Fund.Crawler.Models;
using FundSearcher.Models;

namespace FundSearcher
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FundInfo, FundModel>();
        }
    }
}
