using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Dapper;
using Autowriter.Database;

namespace Autowriter.Pages.Upload
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ViewModel Data { get; private set; }

        public async Task OnGetAsync(Query query)
        {
            Data = await _mediator.Send(query);
        }

        public class ViewModel
        {
            public IEnumerable<Upload> Uploads { get; set; }

            public class Upload
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Text { get; set; }
            }
        }

        public class Query : IRequest<ViewModel> { }

        public class Handler : IRequestHandler<Query, ViewModel>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public async Task<ViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var uploads = await _connection.QueryAsync<ViewModel.Upload>($"SELECT * FROM {TableNames.Uploads}");

                return new ViewModel { Uploads = uploads };
            }
        }
    }
}
