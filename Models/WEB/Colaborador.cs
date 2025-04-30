using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_NEW.Models.WEB   
{
    public class Colaborador
    {
        public string NR_COLABORADOR { get; set; }
        public string NR_EMPRESA { get; set; }
        public string NR_MATRICULA { get; set; }
        public string DT_ADMISSAO { get; set; }
        public string DT_NASCIMENTO { get; set; }
        public string NM_COLABORADOR { get; set; }
        public string NR_CPF { get; set; }
        public string TP_SEXO { get; set; }
        public string NR_RAMAL { get; set; }
        public string NM_EMAIL { get; set; }
        public string NR_GESTOR { get; set; }
        public string NR_FILIAL { get; set; }
        public string NM_FUNCAO_RH { get; set; }
        public string NR_FUNCAO_RH { get; set; }
        public string NR_ATIVIDADE_RH { get; set; } 
        public string NM_SENHA { get; set; }
        public string NM_CONFIRMACAO_SENHA { get; set; }
        public string NR_COORDENADOR { get; set; }
        public string NR_SUPERVISOR { get; set; }
        public string NM_EQUIPE { get; set; }
        public string NR_OLOS { get; set; }
        public string NM_LOGIN_OLOS { get; set; }
        public string TP_TURNO { get; set; }
        public string NM_COORDENADOR { get; set; }
        public string NM_SUPERVISOR { get; set; }
        public int TP_PRIORIDADE_ACESSO { get; set; }
    }
}