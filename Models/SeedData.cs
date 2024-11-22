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
                            EnRole="Field Liaison",
                            FrRole="Relais terrain"
                        },
                        new UserModel {
                            Name="MABESSIMO Thierry",
                            EnRole="Field Liaison",
                            FrRole="Relais terrain"
                        },
                        new UserModel {
                            Name="AMBIL BENI CARDOREL",
                            EnRole="Field Liaison",
                            FrRole="Relais terrain"
                        }
                    );

                    ctx.SaveChanges();
                }


                if ( !ctx.Partners.Any() ) {

                    ctx.Partners.AddRange(
                        new PartnerModel {
                            UrlLink="https://www.worldwildlife.org",
                            Name="WWF",
                            Image="wwf.jpg"
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
                            Image="unesco.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://www.thegef.org",
                            Name="GLOBAL ENVIRONMENT FACILITY (GEF)",
                            Image="gef.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://rightsindevelopment.org/fr/member/green-development-advocates/",
                            Name="GREEN DEVELOPMENT ADVOCATES (GDA)",
                            Image="gba.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://www.google.com/search?client=opera&q=well+grounded&sourceid=opera&ie=UTF-8&oe=UTF-8",
                            Name="WELL GROUNDED",
                            Image="ground.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://www.google.com/search?q=PPI&client=opera&hs=xvQ&sca_esv=581353aac19f9e2d&sxsrf=ADLYWIJjs1a4zoyBzzbrnelsrZhvlw4oGw%3A1732015347001&ei=8nQ8Z6vsPOuuhbIPl_H1yAc&ved=0ahUKEwirrYP8o-iJAxVrV0EAHZd4HXkQ4dUDCA8&uact=5&oq=PPI&gs_lp=Egxnd3Mtd2l6LXNlcnAiA1BQSUjHA1AAWABwAHgBkAEAmAEAoAEAqgEAuAEDyAEA-AEC-AEBmAIAoAIAmAMAkgcAoAcA&sclient=gws-wiz-serp",
                            Name="PPI",
                            Image="ppi.jpg"
                        },
                        new PartnerModel {
                            UrlLink="https://erudef.org",
                            Name="ERUDEF",
                            Image="erudef.jpg"
                        }

                    );

                    ctx.SaveChanges();
                }

                if(!ctx.Opportunities.Any() ) {

                    ctx.Opportunities.AddRange(
                        new OpportunityModel
                        {
                            Job="Teacher",
                            JobDescription="Teach kids and adults on many primary schools subjects",
                            Language="en",
                            JobRequirements="Proficient in Microsoft Office Suite. \n Strong communication skills. \n 2+ years of experience in NGO sector.\n Able to Create and implement communication strategies.",
                            PublishedDate= DateTime.Parse("2023-09-03")
                        },
                        new OpportunityModel
                        {
                            Job="Training Facillitators",
                            JobDescription="Facilitate training sessions for staff and volunteers.",
                            Language="en",
                            JobRequirements="Proficient in Microsoft Office Suite. \n Strong communication skills. \n 2+ years of experience in NGO sector.\n Able to Create and implement communication strategies.",
                            PublishedDate= DateTime.Parse("2024-10-03")
                        },
                        new OpportunityModel
                        {
                            Job="Enseignants",
                            JobDescription="Enseigner aux enfants et aux adultes de nombreuses matières du primaire",
                            Language="fr",
                            JobRequirements="Maîtrise de la suite Microsoft Office. \n Solides compétences en communication. \n Plus de 2 ans d'expérience dans le secteur des ONG. \n Capable de créer et de mettre en œuvre des stratégies de communication.\n avoir un diplome en communications etc.",
                            PublishedDate= DateTime.Parse("2023-09-03")
                        },
                        new OpportunityModel
                        {
                            Job="Animateurs de formation",
                            JobDescription="Animer des séances de formation pour le personnel et les bénévoles.",
                            Language="fr",
                            JobRequirements="Maîtrise de la suite Microsoft Office. \n Solides compétences en communication. \n Plus de 2 ans d'expérience dans le secteur des ONG. \n Capable de créer et de mettre en œuvre des stratégies de communication.\n avoir un diplome dans le domaine de Enseignements.",
                            PublishedDate= DateTime.Parse("2024-10-03")
                        }
                    );
                    ctx.SaveChanges();
                }


                

            }

        }
        
    }
}