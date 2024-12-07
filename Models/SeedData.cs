using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Aafeben.Data;
using System;
using System.Linq;

namespace Aafeben.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider) 
        {
            using ( var ctx = new AafebenDbContext( 
                serviceProvider.GetRequiredService<DbContextOptions<AafebenDbContext>>())
            ) 
            {
                // add staff now if the data base is null for staffs
                if ( !ctx.Users.Any() ) 
                {

                    ctx.Users.AddRange(
                        new UserModel {
                            Name="Akongongol Miranda Mireille",
                            EnRole="Director",
                            FrRole="Directrice",
                            Image="miranda.jpg"
                        },
                        new UserModel { 
                            Name="Mr. Bossi Alain Pierre",
                            EnRole="Administrative and Financial Manager",
                            FrRole="Responsable administratif et financier",
                            Image="bossi.jpg"
                        },
                        new UserModel {
                            Name="Megnitsop Namekong Lucrèce",
                            EnRole="Project Assistant",
                            FrRole="Assistante projet",
                            Image="megnitsop.jpg"
                        },
                        new UserModel {
                            Name="MOPO KAYI Hermann",
                            EnRole="Program Officer",
                            FrRole=" Chargé de programme",
                            Image="herman.jpg"
                        },
                        new UserModel {
                            Name="EBA'A Blaise",
                            EnRole="Communication Officer",
                            FrRole="Chargé de communication",
                            Image="blaise.jpg"
                        },
                        new UserModel {
                            Name="LANGUEL Simon Pierre",
                            EnRole="Field Officer",
                            FrRole="Relais terrain"
                        },
                        new UserModel {
                            Name="MABESSIMO Thierry",
                            EnRole="Field Officer",
                            FrRole="Relais terrain",
                            Image="thierry.jpg"
                        },
                        new UserModel {
                            Name="AMBIL BENI CARDOREL",
                            EnRole="Field Officer",
                            FrRole="Relais terrain",
                            Image="beni.jpg"
                        }
                    );

                    ctx.SaveChanges();
                }


                if ( !ctx.Partners.Any() ) {

                    ctx.Partners.AddRange(
                        new PartnerModel {
                            UrlLink="https://www.worldwildlife.org",
                            Name="WWF",
                            Image="wwf.png"
                        },
                        new PartnerModel {
                            UrlLink="",
                            Name="PARC NATIONAL DE LOBEKE",
                            Image="parc.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://fondationtns.org",
                            Name="FONDATION POUR LE TRI-NATIONAL DE LA SANGHA (FTNS)",
                            Image="ftns.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://www.giz.de/en/worldwide/germany.html#:~:text=In%20Germany%20GIZ%20works%20with,implementing%20their%20international%20cooperation%20projects.",
                            Name="GERMAN COOPERATION implemented by GIZ",
                            Image="giz.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://www.unesco.org",
                            Name="UNESCO",
                            Image="unesco.png"
                        },
                        new PartnerModel {
                            UrlLink="https://www.thegef.org",
                            Name="GLOBAL ENVIRONMENT FACILITY (GEF)",
                            Image="gef.webp"
                        },
                        new PartnerModel {
                            UrlLink="https://rightsindevelopment.org/fr/member/green-development-advocates/",
                            Name="GREEN DEVELOPMENT ADVOCATES (GDA)",
                            Image="gba.png"
                        },
                        new PartnerModel {
                            UrlLink="https://well-grounded.org",
                            Name="WELL GROUNDED",
                            Image="ground.png"
                        },
                        new PartnerModel {
                            UrlLink="https://www.google.com/search?q=PPI&client=opera&hs=xvQ&sca_esv=581353aac19f9e2d&sxsrf=ADLYWIJjs1a4zoyBzzbrnelsrZhvlw4oGw%3A1732015347001&ei=8nQ8Z6vsPOuuhbIPl_H1yAc&ved=0ahUKEwirrYP8o-iJAxVrV0EAHZd4HXkQ4dUDCA8&uact=5&oq=PPI&gs_lp=Egxnd3Mtd2l6LXNlcnAiA1BQSUjHA1AAWABwAHgBkAEAmAEAoAEAqgEAuAEDyAEA-AEC-AEBmAIAoAIAmAMAkgcAoAcA&sclient=gws-wiz-serp",
                            Name="PPI",
                            Image="ppi.png"
                        },
                        new PartnerModel {
                            UrlLink="https://erudef.org",
                            Name="ERUDEF",
                            Image="erudef.jpg"
                        }

                    );
                    ctx.SaveChanges();
                }
            }

        }
        
    }
}