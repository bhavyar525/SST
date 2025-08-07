using AutoMapper;
using ScrumStandUpTrackerProject.DTOs;
using ScrumStandUpTrackerProject.Models;

namespace ScrumStandUpTrackerProject.Mappers
{
    public class EntityMapper: Profile
    {
        public EntityMapper()
        {
            CreateMap<Developer, DeveloperDTO>().ReverseMap();
            CreateMap<DailyStatus, DailyStatusDTO>().ReverseMap();
        }
    }
}