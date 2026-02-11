using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using MapsterMapper;

namespace Forum.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
    }
}
