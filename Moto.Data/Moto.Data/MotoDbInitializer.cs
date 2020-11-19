using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public class MotoDbInitializer : CreateDatabaseIfNotExists<MotoDataContext>
    {
	    protected override void Seed(MotoDataContext context)
	    {
	        IList<Season> defaultSeasons = new List<Season>();

			defaultSeasons.Add(new Season() { Category = Categories.c125, Year = 2020 });
			defaultSeasons.Add(new Season() { Category = Categories.c250, Year = 2020 });
			defaultSeasons.Add(new Season() { Category = Categories.c500, Year = 2020 });

			context.Seasons.AddRange(defaultSeasons);
	
	        base.Seed(context);
	    }
    }
}
