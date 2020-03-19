using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.MVC.Models.DTO
{
    // Este DTO debe tener la misma estructura que el model de la capa Aplication
    // porque este va a ser al que le vamos a pasar esta data.
    public class TransferDto
    {
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public decimal TransferAmount { get; set; }

    }
}
