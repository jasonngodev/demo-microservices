using AutoMapper;

namespace Ordering.Application.Common.Mappings;

public interface IMappFrom<T>
{
    void Mapping(Profile profile) =>
        profile.CreateMap(typeof(T), GetType());
}