using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using EVE01.UI.Clases;
using EVE01.DO.DATA;

namespace EVE01.UI.Models
{
    public class Estadisticas
    {

        #region constantes

        private const string _sqlValores = @"select 
                                            piv.genero,
                                            piv.total total_genero,
                                            cali.con_alimentacion,
                                            sali.sin_alimentacion,
                                            cbus.con_bus,
                                            sbus.sin_bus,
                                            medad.menores_edad,
                                            maedad.mayores_edad
                                            from(
                                            select
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) total
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )piv,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) con_alimentacion
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa,
                                            eve01_inscripcion_opcion op,
                                            eve01_evento_opcion eop
                                            where ins.participante = pa.participante
                                            and ins.evento = op.evento
                                            and op.evento = eop.evento
                                            and op.opcion = eop.opcion
                                            and ins.participante = op.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and eop.descripcion like '%ALIMENTA%'
                                            and op.estado_registro = 'A' --CON ALIMENTACION
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )cali,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) sin_alimentacion
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa,
                                            eve01_inscripcion_opcion op,
                                            eve01_evento_opcion eop
                                            where ins.participante = pa.participante
                                            and ins.evento = op.evento
                                            and op.evento = eop.evento
                                            and op.opcion = eop.opcion
                                            and ins.participante = op.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and eop.descripcion like '%ALIMENTA%'
                                            and op.estado_registro = 'B' --SIN ALIMENTACION
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )sali,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) con_bus
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and exists( --CON BUS
                                            select 1
                                            from eve01_inscripcion_bus bus
                                            where bus.evento = ins.evento
                                            and bus.participante = ins.participante)
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )cbus,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) sin_bus
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and not exists( --SIN BUS
                                            select 1
                                            from eve01_inscripcion_bus bus
                                            where bus.evento = ins.evento
                                            and bus.participante = ins.participante)
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )sbus,
                                            (
                                            select distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) menores_edad
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and to_number(to_char(ins.fecha_inscripcion,'yyyy'))-to_number(to_char(pa.fecha_nacimiento,'yyyy')) < 18 --MENORES DE EDAD
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )medad,
                                            (
                                            select distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) mayores_edad
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and to_number(to_char(ins.fecha_inscripcion,'yyyy'))-to_number(to_char(pa.fecha_nacimiento,'yyyy')) >= 18 --MAYORES DE EDAD
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )maedad
                                            where piv.genero = cali.genero(+)
                                            and piv.genero = sali.genero(+)
                                            and piv.genero = cbus.genero(+)
                                            and piv.genero = sbus.genero(+)
                                            and piv.genero = medad.genero(+)
                                            and piv.genero = maedad.genero(+)";

        private const string _sqlTotales = @"select
                                            uno descripcion,
                                            dos total_gene,
                                            tres total_calimentacion,
                                            cuatro total_salimentacion,
                                            cinco total_cbus,
                                            seis total_sbus,
                                            siete total_menores,
                                            ocho total_mayores
                                            from(
                                            select 
                                            'TOTAL' uno,
                                            null dos,
                                            sum(cali.con_alimentacion) tres,
                                            sum(sali.sin_alimentacion) cuatro,
                                            sum(cbus.con_bus) cinco,
                                            sum(sbus.sin_bus) seis,
                                            sum(medad.menores_edad) siete,
                                            sum(maedad.mayores_edad) ocho
                                            from(
                                            select
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) total
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )piv,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) con_alimentacion
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa,
                                            eve01_inscripcion_opcion op,
                                            eve01_evento_opcion eop
                                            where ins.participante = pa.participante
                                            and ins.evento = op.evento
                                            and op.evento = eop.evento
                                            and op.opcion = eop.opcion
                                            and ins.participante = op.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and eop.descripcion like '%ALIMENTA%'
                                            and op.estado_registro = 'A' --CON ALIMENTACION
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )cali,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) sin_alimentacion
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa,
                                            eve01_inscripcion_opcion op,
                                            eve01_evento_opcion eop
                                            where ins.participante = pa.participante
                                            and ins.evento = op.evento
                                            and op.evento = eop.evento
                                            and op.opcion = eop.opcion
                                            and ins.participante = op.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and eop.descripcion like '%ALIMENTA%'
                                            and op.estado_registro = 'B' --SIN ALIMENTACION
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )sali,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) con_bus
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and exists( --CON BUS
                                            select 1
                                            from eve01_inscripcion_bus bus
                                            where bus.evento = ins.evento
                                            and bus.participante = ins.participante)
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )cbus,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) sin_bus
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and not exists( --SIN BUS
                                            select 1
                                            from eve01_inscripcion_bus bus
                                            where bus.evento = ins.evento
                                            and bus.participante = ins.participante)
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )sbus,
                                            (
                                            select distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) menores_edad
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and to_number(to_char(ins.fecha_inscripcion,'yyyy'))-to_number(to_char(pa.fecha_nacimiento,'yyyy')) < 18 --MENORES DE EDAD
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )medad,
                                            (
                                            select distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) mayores_edad
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and to_number(to_char(ins.fecha_inscripcion,'yyyy'))-to_number(to_char(pa.fecha_nacimiento,'yyyy')) >= 18 --MAYORES DE EDAD
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )maedad
                                            where piv.genero = cali.genero(+)
                                            and piv.genero = sali.genero(+)
                                            and piv.genero = cbus.genero(+)
                                            and piv.genero = sbus.genero(+)
                                            and piv.genero = medad.genero(+)
                                            and piv.genero = maedad.genero(+)
                                            union all
                                            select 
                                            'TOTALES' uno,
                                            sum(piv.total) dos,
                                            null tres,
                                            sum(cali.con_alimentacion) + sum(sali.sin_alimentacion) cuatro,
                                            null cinco,
                                            sum(cbus.con_bus) + sum(sbus.sin_bus) seis,
                                            null siete,
                                            sum(medad.menores_edad) + sum(maedad.mayores_edad) ocho
                                            from(
                                            select
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) total
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )piv,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) con_alimentacion
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa,
                                            eve01_inscripcion_opcion op,
                                            eve01_evento_opcion eop
                                            where ins.participante = pa.participante
                                            and ins.evento = op.evento
                                            and op.evento = eop.evento
                                            and op.opcion = eop.opcion
                                            and ins.participante = op.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and eop.descripcion like '%ALIMENTA%'
                                            and op.estado_registro = 'A' --CON ALIMENTACION
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )cali,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) sin_alimentacion
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa,
                                            eve01_inscripcion_opcion op,
                                            eve01_evento_opcion eop
                                            where ins.participante = pa.participante
                                            and ins.evento = op.evento
                                            and op.evento = eop.evento
                                            and op.opcion = eop.opcion
                                            and ins.participante = op.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and eop.descripcion like '%ALIMENTA%'
                                            and op.estado_registro = 'B' --SIN ALIMENTACION
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )sali,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) con_bus
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and exists( --CON BUS
                                            select 1
                                            from eve01_inscripcion_bus bus
                                            where bus.evento = ins.evento
                                            and bus.participante = ins.participante)
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )cbus,
                                            (
                                            select 
                                            distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) sin_bus
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and ins.estado_registro = 'A'
                                            and not exists( --SIN BUS
                                            select 1
                                            from eve01_inscripcion_bus bus
                                            where bus.evento = ins.evento
                                            and bus.participante = ins.participante)
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )sbus,
                                            (
                                            select distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) menores_edad
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and to_number(to_char(ins.fecha_inscripcion,'yyyy'))-to_number(to_char(pa.fecha_nacimiento,'yyyy')) < 18 --MENORES DE EDAD
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )medad,
                                            (
                                            select distinct(decode(pa.genero,'M','HOMBRES','F','MUJERES'))genero,
                                            count(ins.participante) mayores_edad
                                            from
                                            eve01_inscripcion ins,
                                            eve01_participante pa
                                            where ins.participante = pa.participante
                                            and ins.evento = :evento
                                            and to_number(to_char(ins.fecha_inscripcion,'yyyy'))-to_number(to_char(pa.fecha_nacimiento,'yyyy')) >= 18 --MAYORES DE EDAD
                                            and ins.estado_registro = 'A'
                                            group by (decode(pa.genero,'M','HOMBRES','F','MUJERES'))
                                            )maedad
                                            where piv.genero = cali.genero(+)
                                            and piv.genero = sali.genero(+)
                                            and piv.genero = cbus.genero(+)
                                            and piv.genero = sbus.genero(+)
                                            and piv.genero = medad.genero(+)
                                            and piv.genero = maedad.genero(+))";

        #endregion

        #region propiedades

        public decimal evento { get; set; }

        public string genero { get; set; }
        public decimal? total_genero { get; set; }
        public decimal? con_alimentacion { get; set; }
        public decimal? sin_alimentacion { get; set; }
        public decimal? con_bus { get; set; }
        public decimal? sin_bus { get; set; }
        public decimal? menores_edad { get; set; }
        public decimal? mayores_edad { get; set; }

        public string descripcion { get; set; }
        public decimal? total_gene { get; set; }
        public decimal? total_calimentacion { get; set; }
        public decimal? total_salimentacion { get; set; }
        public decimal? total_cbus { get; set; }
        public decimal? total_sbus { get; set; }
        public decimal? total_menores { get; set; }
        public decimal? total_mayores { get; set; }


        #endregion


        #region Metodos Publicos

        public Respuesta<List<Estadisticas>> Valores()
        {
            Respuesta<List<Estadisticas>> result = new Respuesta<List<Estadisticas>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de Datos";
            result.data = new List<Estadisticas>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    StringBuilder strSqlValores = new StringBuilder();
                    strSqlValores.Append(_sqlValores);

                    var list = db.Database.SqlQuery<Estadisticas>(strSqlValores.ToString(), new object[] { MvcApplication.idEvento }).ToList<Estadisticas>();

                    if (list != null)
                    {
                        result.data = list;
                    }
                    else
                    {
                        result.data = new List<Estadisticas>();
                    }                    
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener los datos de los Valores de Estadistica, ref: " + ex.ToString();
                return result;
            }

        }

        public Respuesta<List<Estadisticas>> Totales()
        {
            Respuesta<List<Estadisticas>> result = new Respuesta<List<Estadisticas>>();
            result.codigo = 1;
            result.mensaje = "Ocurrio un error en Base de Datos";
            result.data = new List<Estadisticas>();

            try
            {
                using (var db = new EntitiesEVE01())
                {
                    StringBuilder strSqlTotales = new StringBuilder();
                    strSqlTotales.Append(_sqlTotales);

                    var list = db.Database.SqlQuery<Estadisticas>(strSqlTotales.ToString(), new object[] { MvcApplication.idEvento }).ToList<Estadisticas>();

                    if (list != null)
                    {
                        result.data = list;
                    }
                    else
                    {
                        result.data = new List<Estadisticas>();
                    }
                }
                result.codigo = 0;
                result.mensaje = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.codigo = -1;
                result.mensaje = "Ocurrio una excepcion al obtener los datos de los Valores de Estadistica, ref: " + ex.ToString();
                return result;
            }

        }

        #endregion



    }
}