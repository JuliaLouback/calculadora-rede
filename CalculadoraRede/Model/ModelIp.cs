using CalculadoraRede.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraRede.Model
{
    // JULIA LOUBACK RIBEIRO (2019102092)
    // ERIKA CRISTINE HATSUGAI (2019101301)
    // GREICI EVANI DE SOUZA (2019102652)

    class ModelIp
    {
        public string IpBase { get; set; }

        public char Classe { get; set; }

        public string MascaraPadrao { get; set; }

        public string IpSoma { get; set; }

        public int[] ArrayPotencia2 = new int[8] { 128, 64, 32, 16, 8, 4, 2, 1 };

        Host hosts = new Host();

        List<Host> listaHost = new List<Host>();

        List<Subrede> listaSubrede = new List<Subrede>();

        public void adicionarIpBase(string ipBase){
            IpBase = ipBase;
        }

        public int numSubrede = 0;

        public void adicionarHost(int quantidadeHost) 
        {

            Host hosts = new Host();

            hosts.Hosts = quantidadeHost;
            hosts.Mais2 = quantidadeHost + 2;

            int mais2 = quantidadeHost + 2;

            int potencia = 0;

            int i = 0;

            while (mais2 >= Math.Pow(2, i))
            {
                if (mais2 == Math.Pow(2, i))
                {
                    potencia = Convert.ToInt32(Math.Pow(2, i));
                    break;
                }
                else
                {
                    i++;
                    potencia = Convert.ToInt32(Math.Pow(2, i));
                }
            }

            hosts.Potencia = i;

            hosts.Total = potencia;

            listaHost.Add(hosts);
        }

        public List<Host> retornarListaHost(){

            return listaHost.OrderByDescending(x => x.Total).ToList();
        }

        public void calcularSubrede(){

            IpSoma = IpBase;

            string[] ArrayVerifica = new string[4];  
            string value = IpSoma.Replace('.', ' ');
            ArrayVerifica = value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // cria um array de strings

            int ParteUm = Convert.ToInt32(ArrayVerifica[0]);
            int ParteDois = Convert.ToInt32(ArrayVerifica[1]);
            int ParteTres = Convert.ToInt32(ArrayVerifica[2]);
            int ParteQuatro = Convert.ToInt32(ArrayVerifica[3]);

            int TotalHost = 0;

            List<Host> lista = listaHost.OrderByDescending(x => x.Total).ToList();
            foreach (var item in lista)
            {
                Subrede subrede = new Subrede();

                subrede.NumeroSubrede = numSubrede;
                subrede.IdRede = IpSoma;
                subrede.TotalHost = TotalHost;

                IpSoma = IpBase; // primeira subrede recebe como Id de rede o Ip base

                ParteQuatro = ParteQuatro + item.Total;

                if (Classe == 'A')
                {
                    while (ParteQuatro >= 256)
                    {
                        ParteQuatro = ParteQuatro - 256;
                        ParteTres++;
                    }

                    while (ParteTres >= 256)
                    {
                        ParteTres = ParteTres - 256;
                        ParteDois++;
                    }

                    IpSoma = ParteUm + "." + ParteDois + "." + ParteTres + "." + ParteQuatro;

                } else if (Classe == 'B')
                {
                    while (ParteQuatro >= 256)
                    {
                        ParteQuatro = ParteQuatro - 256;
                        ParteTres++;
                    }

                    IpSoma = ParteUm + "." + ParteDois + "." + ParteTres + "." + ParteQuatro;
                }
                else
                {
                    IpSoma = ParteUm + "." + ParteDois + "." + ParteTres + "." + (ParteQuatro);
                }

                subrede.Broadcast = acharBroad(IpSoma);
                subrede.MascaraCustomizada = calcularMascaraCustom(item.Total);
               
                listaSubrede.Add(subrede);
                numSubrede++;
                TotalHost += item.Total;
            }

        }

        public string acharBroad(string tamanhorede)
        {
            string[] ArrayVerifica = new string[4];
            string value = tamanhorede.Replace('.', ' ');
            ArrayVerifica = value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            int ParteUm = Convert.ToInt32(ArrayVerifica[0]);
            int ParteDois = Convert.ToInt32(ArrayVerifica[1]);
            int ParteTres = Convert.ToInt32(ArrayVerifica[2]);
            int ParteQuatro = Convert.ToInt32(ArrayVerifica[3]);

            ParteQuatro = ParteQuatro - 1;
            if (ParteQuatro == -1)
            {
                ParteQuatro = 255;
                ParteTres = ParteTres - 1;

                if (ParteTres == -1)
                {
                    ParteTres = 255;
                    ParteDois = ParteDois - 1;
                }
            }

            string broad = ParteUm + "." + ParteDois + "." + ParteTres + "." + ParteQuatro;
            return broad;
        }

        public List<Subrede> retornarSubrede()
        {
            return listaSubrede;
        }

        public string converterBinario(string valorIp) {

            string[] ArrayNumeros = new string[4];
            string value = valorIp.Replace('.', ' ');
            ArrayNumeros = value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            string aux = "";

            for (int i = 0; i < ArrayNumeros.Length; i++) {

                int numero = Convert.ToInt32(ArrayNumeros[i]);

                for (int k = 0; k < ArrayPotencia2.Length; k++) {

                    if (numero == 0) {

                        aux = aux + "0";

                    } else if (numero >= ArrayPotencia2[k]) {

                        numero = numero - ArrayPotencia2[k];
                        aux = aux + "1";

                    } else {
                        aux = aux + "0";
                    }
                }

                if (i != 3) {
                    aux = aux + ".";
                }
            }

            return aux;
        }

        public string calcularMascaraCustom(int totalHost){

            string[] ArrayNumeros = new string[4];
            string value = MascaraPadrao.Replace('.', ' ');
            ArrayNumeros = value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            int ParteUm = Convert.ToInt32(ArrayNumeros[0]);
            int ParteDois = Convert.ToInt32(ArrayNumeros[1]);
            int ParteTres = Convert.ToInt32(ArrayNumeros[2]);
            int ParteQuatro = Convert.ToInt32(ArrayNumeros[3]);

            if (totalHost < 256)
            {
                ParteQuatro = 256 - totalHost;
            } else if (totalHost >= 256 && totalHost < 65536)
            {
                ParteTres = (65536 - totalHost) / 256;
            } else
            {
                ParteDois = (((16777216 - totalHost) / 256) /256);
            }

            return ParteUm+"."+ParteDois+"."+ParteTres+"."+ParteQuatro;
        }

        public void excluir(int item){

            listaHost.RemoveAt(item);

        }

        public void limpar()
        {
            IpBase = "";
            Classe = ' ';
            MascaraPadrao = "";
            listaHost.Clear();
            listaSubrede.Clear();
            IpSoma = "";
            numSubrede = 0;
        }
    }
}
