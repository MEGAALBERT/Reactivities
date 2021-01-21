using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest 
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var activity = await _context.Activity.FindAsync(request.Id);

                if(activity == null)
                {
                    throw new Exception("could not find activity");
                }

                _context.Remove(activity);

                var succcess = await _context.SaveChangesAsync() > 0;

                if (succcess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
