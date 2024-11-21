﻿using Gatam.Application.CQRS;
using AutoMapper;
using Gatam.Domain;
using System;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.CQRS.Questions;
namespace Gatam.Application
{
    public class Mapping:Profile
    {
        public Mapping()
        {
                CreateMap<ApplicationUser, UserDTO>();
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<ApplicationModule, ModuleDTO>();
        
        CreateMap<Question, QuestionDTO>()
            .ForMember(dest => dest.QuestionSetting, 
                      opt => opt.MapFrom(src => src.UserModuleQuestionSettings.FirstOrDefault()));

         CreateMap<UserModule, UserModuleDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
            .ForMember(dest => dest.QuestionSettings, opt => opt.MapFrom(src => src.QuestionSettings));
        CreateMap<UserModuleQuestionSetting, QuestionSettingDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
            .ForMember(dest => dest.QuestionTitle, opt => opt.MapFrom(src => src.Question.QuestionTitle))
            .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Question.QuestionType));

        CreateMap<QuestionSettingDTO, UserModuleQuestionSetting>()
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.UserModule, opt => opt.Ignore());

        CreateMap<CreateSettingCommand, UserModuleQuestionSetting>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.UserModule, opt => opt.Ignore());
        }
    }
}
