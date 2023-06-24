namespace DishesAPI.Profiles
{
    using AutoMapper;
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Entities.Ingredient, Models.Ingredient>().ForMember(
                i => i.DishId,
                o => o.MapFrom(s => s.Dishes.First().Id));
        }
    }
}
