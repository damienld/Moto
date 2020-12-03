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

			defaultSeasons.Add(new Season() { Category = Categories.Moto3, Year = 2020 });
			defaultSeasons.Add(new Season() { Category = Categories.Moto2, Year = 2020 });
			defaultSeasons.Add(new Season() { Category = Categories.MotoGP, Year = 2020 });

			context.Seasons.AddRange(defaultSeasons);
	
	        base.Seed(context);
	    }
    }
}
