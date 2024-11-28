using Gatam.Application.CQRS;
using AutoMapper;
using Gatam.Domain;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
namespace Gatam.Application
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<ModuleDTO, ApplicationModule>();
        CreateMap<ApplicationModule, ModuleDTO>();


         CreateMap<Question, QuestionDTO>()
            .ForMember(dest => dest.UserQuestion, 
                      opt => opt.MapFrom(src => src.UserQuestions.FirstOrDefault()));
         
            CreateMap<UserModule, UserModuleDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))

            .ForMember(dest => dest.UserQuestions, opt => opt.MapFrom(src => src.UserQuestions))
            .ForMember(dest => dest.UserGivenAnswers, opt => opt.MapFrom(src => src.UserGivenAnswers));

            CreateMap<UserQuestion, UserQuestionDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
            .ForMember(dest => dest.QuestionTitle, opt => opt.MapFrom(src => src.Question.QuestionTitle))
            .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Question.QuestionType))
            .ForMember(dest => dest.QuestionPriority, opt => opt.MapFrom(src => (QuestionPriority)src.QuestionPriority));

            CreateMap<UserQuestionDTO, UserQuestion>()
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.UserModule, opt => opt.Ignore());

        CreateMap<CreateSettingCommand, UserQuestion>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.UserModule, opt => opt.Ignore());
        }
    }
}
