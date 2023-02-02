using Newtonsoft.Json;
using PizzasApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PizzasApp
{
    
    public partial class MainPage : ContentPage
    {
        List<Pizza> pizzas;
        string pissaJson = "";

        enum e_tri
        {
            TRI_AUCUN,
            TRI_NOM,
            TRI_PRIX
        }
        e_tri tri = e_tri.TRI_AUCUN;

        //on va rendre notre application persistante
        //application.current.properties

        //key  string

        const string KEY_TRI = "tri";
        string tempFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp");
        string jsonFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pizzas.json");
        public MainPage()
        {
            if (Application.Current.Properties.ContainsKey(KEY_TRI))
            {
                tri = (e_tri) Application.Current.Properties[KEY_TRI];
                sortButton.Source = GetImageFromSource(tri);
            }
           
            InitializeComponent();
            //  pizzas = new List<Pizza>();
           
            listeview.RefreshCommand = new Command((obj) =>
            {

                DownloadData((pizzas) => {
                    if(pizzas != null)
                    {
                        listeview.ItemsSource = GetPizzaFromTri(tri, pizzas);
                    }
                    
                    listeview.IsRefreshing = false;
                });


            });
            listeview.IsVisible = false;
            waitlayout.IsVisible = true;
            if (File.Exists(jsonFileName))
            {
              string pizzaJson=  File.ReadAllText(jsonFileName);
                if (!string.IsNullOrEmpty(pizzaJson))
                {
                    pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pissaJson);
                    listeview.ItemsSource = GetPizzaFromTri(tri, pizzas);

                    listeview.IsVisible = true;
                    waitlayout.IsVisible = false;
                }
               
            }

            Console.WriteLine("Etape 1");

            // telechargement de mes données avec ma fonction download data
            DownloadData((pizzas) => {
                if(pizzas != null)
                {
                    listeview.ItemsSource = GetPizzaFromTri(tri, pizzas);
                }
                
                listeview.IsVisible = true;
                waitlayout.IsVisible = false;
                //listeview.IsRefreshing = false; 
            });

            Console.WriteLine("Etape 4");


            //string pissaJson = "https://drive.google.com/uc?export=download&id=1GkrLGGDQDAxqJc5MX1cuPQKtMoNNNf5Y";
            // string pissaJson = "[\r\n\t{ \"nom\": \"4 fromages\", \"ingredients\": [ \"cantal\", \"mozzarella\", \"fromage de chèvre\", \"gruyère\" ], \"prix\": 11, \"imageUrl\": \"https://www.galbani.fr/wp-content/uploads/2017/07/pizza_filant_montage_2_3.jpg\"},\r\n\t{ \"nom\": \"tartiflette\", \"ingredients\": [ \"pomme de terre\", \"oignons\", \"crème fraiche\", \"lardons\", \"mozzarella\" ], \"prix\": 14, \"imageUrl\": \"https://cdn.pizzamatch.com/1/35/1375105305-pizza-napolitain-630.JPG?1375105310\"},\r\n\t{ \"nom\": \"margherita\", \"ingredients\": [ \"sauce tomate\", \"mozzarella\", \"basilic\" ], \"prix\": 7, \"imageUrl\": \"https://www.misteriosocultos.com/wp-content/uploads/2018/12/pizza.jpg\"},\r\n\t{ \"nom\": \"indienne\", \"ingredients\": [ \"curry\", \"mozzarella\", \"poulet\", \"poivron\", \"oignon\", \"coriandre\" ], \"prix\": 10, \"imageUrl\": \"https://assets.afcdn.com/recipe/20160519/15342_w1024h768c1cx3504cy2338.jpg\"},\r\n\t{ \"nom\": \"mexicaine\", \"ingredients\": [ \"boeuf\", \"mozzarella\", \"maïs\", \"tomates\", \"oignon\", \"coriandre\" ], \"prix\": 13, \"imageUrl\": \"https://fac.img.pmdstatic.net/fit/http.3A.2F.2Fprd2-bone-image.2Es3-website-eu-west-1.2Eamazonaws.2Ecom.2FFAC.2Fvar.2Ffemmeactuelle.2Fstorage.2Fimages.2Fminceur.2Fastuces-minceur.2Fminceur-choix-pizzeria-47943.2F14883894-1-fre-FR.2Fminceur-comment-faire-les-bons-choix-a-la-pizzeria.2Ejpg/750x562/quality/80/crop-from/center/minceur-comment-faire-les-bons-choix-a-la-pizzeria.jpeg\"},\r\n\t{ \"nom\": \"chèvre et miel\", \"ingredients\": [ \"miel\", \"mozzarella\", \"fromage de chèvre\", \"roquette\"], \"prix\": 10, \"imageUrl\": \"http://gfx.viberadio.sn/var/ezflow_site/storage/images/news/conso-societe/les-4-aliments-a-eviter-de-consommer-le-soir-00018042/155338-1-fre-FR/Les-4-aliments-a-eviter-de-consommer-le-soir.jpg\"},\r\n\t{ \"nom\": \"napolitaine\", \"ingredients\": [ \"sauce tomate\", \"mozzarella\", \"anchois\", \"câpres\"], \"prix\": 9, \"imageUrl\": \"https://www.fourchette-et-bikini.fr/sites/default/files/pizza_tomate_mozzarella.jpg\"},\r\n\t{ \"nom\": \"kebab\", \"ingredients\": [ \"poulet\", \"oignons\", \"sauce tomate\", \"sauce kebab\", \"mozzarella\"], \"prix\": 11, \"imageUrl\": \"https://res.cloudinary.com/serdy-m-dia-inc/image/upload/f_auto/fl_lossy/q_auto:eco/x_0,y_0,w_3839,h_2159,c_crop/w_576,h_324,c_scale/v1525204543/foodlavie/prod/recettes/pizza-au-chorizo-et-fromage-cheddar-en-grains-2421eadb\"},\r\n\t{ \"nom\": \"louisiane\", \"ingredients\": [ \"poulet\", \"champignons\", \"poivrons\", \"oignons\", \"sauce tomate\", \"mozzarella\"], \"prix\": 12, \"imageUrl\": \"http://www.fraichementpresse.ca/image/policy:1.3167780:1503508221/Pizza-dejeuner-maison-basilic-et-oeufs.jpg?w=700&$p$w=13b13d9\"},\r\n\t{ \"nom\": \"orientale\", \"ingredients\": [ \"merguez\", \"champignons\", \"sauce tomate\", \"mozzarella\"], \"prix\": 11, \"imageUrl\": \"https://www.atelierdeschefs.com/media/recette-e30299-pizza-pepperoni-tomate-mozza.jpg\"},\r\n\t{ \"nom\": \"hawaïenne\", \"ingredients\": [ \"jambon\", \"ananas\", \"sauce tomate\", \"mozzarella\"], \"prix\": 12, \"imageUrl\": \"https://www.atelierdeschefs.com/media/recette-e16312-pizza-quatre-saisons.jpg\"},\r\n\t{ \"nom\": \"reine\", \"ingredients\": [ \"jambon\", \"champignons\", \"sauce tomate\", \"mozzarella\"], \"prix\": 8, \"imageUrl\": \"https://static.cuisineaz.com/400x320/i96018-pizza-reine.jpg\"},\r\n]";
            //  pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pissaJson);


            /* pizzas.Add(new Pizza { nom = "Macedoine", prix = 7, ingredients = new string[] { "tomate", "fromage", "haricot_vert", "sardine", "chevre de miel", "vegetarienne","roblecon","mayonnaise","steak","burger","ketchup" } });
             pizzas.Add(new Pizza { nom = "Végetarienne", prix = 14, ingredients = new string[] { "carotte", "mandarine", "shawarma", "biscuit" } });
             pizzas.Add(new Pizza { nom = "Velodrome", prix = 12, ingredients = new string[] { "carotte", "mangue", "shawarma", } });
             pizzas.Add(new Pizza { nom = "Malgache", prix = 10, ingredients = new string[] { "carotte", "mandarine", "shawarma", "biscuit" } });*/
            //listeview.ItemsSource = pizzas;

        }

        private void sortButton_Clicked(object sender, EventArgs e)
        {
            if (tri == e_tri.TRI_AUCUN)
            {
                tri = e_tri.TRI_NOM;
            }
            else if (tri == e_tri.TRI_NOM)
            {
                tri = e_tri.TRI_PRIX;
            }
            else if (tri == e_tri.TRI_PRIX)
            {
                tri = e_tri.TRI_AUCUN;
            }
            sortButton.Source= GetImageFromSource(tri);
            listeview.ItemsSource = GetPizzaFromTri(tri, pizzas);
            Application.Current.Properties[KEY_TRI] = (int)tri;
            Application.Current.SavePropertiesAsync();
        }
        private string GetImageFromSource(e_tri t)
         {
             switch (t)
             {
                 case e_tri.TRI_NOM:
                     return "sort_nom.png";

                 case e_tri.TRI_PRIX:
                     return "sort_prix.png";
             }
             return "sort_none.png";
         }

        private List<Pizza> GetPizzaFromTri(e_tri t,List<Pizza> l)
        {
            if (l == null)
            {
                return null;
            }
            switch (t)
            {
                case e_tri.TRI_NOM:
                    {
                        List<Pizza> ret = new List<Pizza>(l);
                        ret.Sort( (p1, p2) => {
                            return p1.Titre.CompareTo(p2.Titre);
                        });

                        return ret;
                    }
                    

                case e_tri.TRI_PRIX:
                    {
                        List<Pizza> ret = new List<Pizza>(l);
                        ret.Sort((p1, p2) =>
                        {
                            return p2.prix.CompareTo(p1.prix);
                        });
                        return ret;
                    }
            }
            return l;
        }

        //creation de la fonction pour télécharger les données

         void DownloadData(Action <List<Pizza>> action )
        {
            const string URL = "https://drive.google.com/uc?export=download&id=1GkrLGGDQDAxqJc5MX1cuPQKtMoNNNf5Y";
            
            using (var webclient = new WebClient())
            {
                try
                {
                    //thread Main
                    // pissaJson = webclient.DownloadString(URL);
                    Console.WriteLine("Etape 2");
                    webclient.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                    {
                        Exception ex = e.Error;
                        if(ex== null)
                        {
                            File.Copy(tempFileName, jsonFileName,true);
                            pissaJson = File.ReadAllText(jsonFileName);
                            pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pissaJson);
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(async() =>
                            {
                               await DisplayAlert("Erreur", "votre lien est éronné " + ex.Message, "OK");

                                action.Invoke(null);
                            });
                        }
                        Console.WriteLine("Etape 5");
                        //pissaJson = e.Result;

                         //pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pissaJson);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            /*listeview.ItemsSource = pizzas;
                            listeview.IsVisible = true;
                            waitlayout.IsVisible = false;
                            listeview.IsRefreshing = false;*/
                            action.Invoke(pizzas);
                        });
                    };




                    Console.WriteLine("Etape 3");
                  //  webclient.DownloadStringAsync(new Uri(URL));
                    webclient.DownloadFileAsync(new Uri(URL),tempFileName);
                }
                catch (Exception ex)
                {
                    //thread réseau
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Erreur", "votre lien est éronné " + ex.Message, "OK");

                        action.Invoke(null);
                    });
                    return;

                }


            }
        }

    }
}
