using CalculadoraRede.Entidade;
using CalculadoraRede.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraRede.Controler
{
    class Controle
    {
        ModelIp modelIp = new ModelIp();

        public string adicionarBase(string ipBase){

            string[] ArrayVerifica = new string[4];

            string value = ipBase.Replace('.', ' ');
            ArrayVerifica = value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (Convert.ToInt32(ArrayVerifica[0]) < 224){

                modelIp.adicionarIpBase(ipBase);

                if (Convert.ToInt32(ArrayVerifica[0]) < 128){
                    modelIp.Classe = 'A';
                    modelIp.MascaraPadrao = "255.0.0.0";
                }
                else if (Convert.ToInt32(ArrayVerifica[0]) >= 128 && Convert.ToInt32(ArrayVerifica[0]) < 192){
                    modelIp.Classe = 'B';
                    modelIp.MascaraPadrao = "255.255.0.0";
                }
                else if (Convert.ToInt32(ArrayVerifica[0]) >= 192 && Convert.ToInt32(ArrayVerifica[0]) < 224){
                    modelIp.Classe = 'C';
                    modelIp.MascaraPadrao = "255.255.255.0";
                }

                return "Valido";
            }
            else
            {
                if (ArrayVerifica.Length < 4){

                    return "Invalido";

                } else
                {
                    return "Maior";

                }
            }
        }

        public void adicionarHost(int quantidadeHost){

            modelIp.adicionarHost(quantidadeHost);
        }

        public List<Host> retornarListaHost(){

            return modelIp.retornarListaHost();
        }

        public string retornarIpBase(){

            return modelIp.IpBase;
        }

        public char retornarClasse(){

            return modelIp.Classe;
        }

        public string retornarMascaraPadrao(){

            return modelIp.MascaraPadrao;
        }

        public bool verificarQuantidadeHost(){

            int total = 0;

            List<Host> listaHost = modelIp.retornarListaHost();

            foreach (var listinha in listaHost)
            {
                total = total + listinha.Total;
            }

            if ((modelIp.Classe == 'A'  && total <= 16777216) || (modelIp.Classe == 'B' && total <= 65536) || (modelIp.Classe == 'C' && total <= 256))
            {
                modelIp.calcularSubrede();
                return true;
            }
            {
                return false;
            }
        }

        public List<Subrede> retornarSubrede(){

            return modelIp.retornarSubrede();
        }

        public string converterBinario(string ip){

            return modelIp.converterBinario(ip);
        }

        public void excluir(int item){

            modelIp.excluir(item);
        }

        public void limpar(){

            modelIp.limpar();
        }
    }
}
