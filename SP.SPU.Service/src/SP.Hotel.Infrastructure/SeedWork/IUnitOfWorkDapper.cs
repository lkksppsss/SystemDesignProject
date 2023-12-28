using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.SPU.Infrastructure.SeedWork;

public interface IUnitOfWorkDapper
{

    /// <summary>
    /// Get current db connection - Master
    /// </summary>
    IDbConnection Master { get; }

    /// <summary>
    /// Get current db connection - Slave
    /// </summary>
    IDbConnection Slave { get; }

    /// <summary>
    /// Get current db transaction
    /// </summary>
    IDbTransaction Transaction { get; }

    /// <summary>
    /// Begin transaction 
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commit data changes.
    /// </summary>
    /// <returns></returns>
    void Commit();

    /// <summary>
    /// Rollback change datas.
    /// </summary>
    /// <returns></returns>
    void RollBack();
}
