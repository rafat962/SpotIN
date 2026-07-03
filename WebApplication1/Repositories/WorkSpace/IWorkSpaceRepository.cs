using System.Collections.Generic;
using WebApplication1.Models.Domain.WorkSpaces;
using WebApplication1.Repositories.WorkSpaces;

namespace WebApplication1.Repositories.WorkSpaces
{
    public interface IWorkSpaceRepository
    {
        public WorkSpace? GetWorkSpaceByOwnerId(string ownerId);
    }
}