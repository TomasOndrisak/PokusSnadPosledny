﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastruktura.Models;

namespace Infrastruktura.Repositories
{
    public class ZamestnanecRepository
    {
        private readonly ZamestnanciContext _context;

        public ZamestnanecRepository(ZamestnanciContext context)
        {
            _context = context;
        }

        public async Task <IEnumerable<Zamestnanci>> GetZamestnanci()
        {
            return await _context.Zamestnanci.ToListAsync();
        }

        public async Task <Zamestnanci> GetZamestnanciId(int id)
        {
            var zamestnanci = await _context.Zamestnanci.FindAsync(id);
            return zamestnanci;

        }

        public async Task Put(int id, Zamestnanci zamestnanci)
        {
             
             
            var zamestnanecPred = from zam in _context.Zamestnanci
                                where zam.ZamestnanecId == id
                                select zam.IdPozicie;
            var zamestnanecPre = await zamestnanecPred.FirstOrDefaultAsync();

            
            
                
              _context.Entry(zamestnanci).State = EntityState.Modified;
              
              
            try
             {
                 await _context.SaveChangesAsync();
                    // PredoslePozicie zmena = new PredoslePozicie();
                    // zmena.ZamestnanecId=zamestnanec.ZamestnanecId;
                    // zmena.PoziciaId=zamestnanec.IdPozicie;
                    // zmena.DatumNastupu=zamestnanec.DatumNastupu;
                    // zmena.DatumUkoncenia=DateTime.Now;
                    //  _context.Predoslepozicie.Add(zmena);
                    
                    var zamestnanecPo = from pozmene in _context.Zamestnanci
                                        where pozmene.ZamestnanecId == id
                                        select pozmene.IdPozicie;

                    if(zamestnanecPred != zamestnanecPo)
                    { 
                        _context.Predoslepozicie.Add(new PredoslePozicie{ZamestnanecId = zamestnanci.ZamestnanecId, PoziciaId = zamestnanci.IdPozicie, DatumNastupu = zamestnanci.DatumNastupu, DatumUkoncenia = DateTime.Now});
                         await _context.SaveChangesAsync();
                    }

                 
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!ZamestnanciExists(id))
                 {
                    throw new ArgumentOutOfRangeException(nameof(id), "Nespravne ID");
                 }
             }
         }

            private bool ZamestnanciExists(int id)
            {
                return _context.Zamestnanci.Any(e => e.ZamestnanecId == id);
            }
       
        public async Task PostZamestnanci(Zamestnanci zamestnanci)
        {
             _context.Zamestnanci.Add(zamestnanci);
             await _context.SaveChangesAsync();
        }

         public async Task DeleteZamestnanci(int id, bool archivovat)
         {

             var zamestnanci = await _context.Zamestnanci.FindAsync(id);

                if (archivovat)
                {
                    zamestnanci.Archivovany = true;
                }
                else 
                {
                    _context.Zamestnanci.Remove(zamestnanci);
                }

                    await _context.SaveChangesAsync();
         }
 

    }
}