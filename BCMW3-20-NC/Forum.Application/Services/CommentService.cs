using Forum.Application.Contracts.Service;
using MapsterMapper;

namespace Forum.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentService(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }
    }
}
