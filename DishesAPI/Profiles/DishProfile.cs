namespace DishesAPI.Profiles
{
    using AutoMapper;
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<Entities.Dish, Models.Dish>().ReverseMap();
            CreateMap<Models.DishForCreation, Entities.Dish>();
            CreateMap<Models.DishForUpdate, Entities.Dish>();
        }
    }
}