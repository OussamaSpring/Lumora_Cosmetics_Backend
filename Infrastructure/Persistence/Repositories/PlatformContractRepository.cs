using Npgsql;
using Application.Interfaces;
using Domain.Entities.VendorVerification;



namespace Infrastructure.Persistence.Repositories
{
    class PlatformContractRepository : IPlatformContractRepository
    {

        #region GetData
        public async Task<Contract> GetContractAsync(short contractId)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                SELECT * FROM contract 
                WHERE id = @contractId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@contractId", contractId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Contract
                            {
                                Id = reader.GetInt16(0),
                                Title = reader.GetString(1),
                                Text = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public async Task<IEnumerable<Contract>> GetAllContractsAsync()
        {
            var contracts = new List<Contract>();
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                SELECT * FROM contract";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            contracts.Add(new Contract
                            {
                                Id = reader.GetInt16(0),
                                Title = reader.GetString(1),
                                Text = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return contracts;
        }

        public async Task<IEnumerable<VendorContract>> GetVendorContractsAsync(Guid vendorId)
        {
            var vendorContracts = new List<VendorContract>();
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                SELECT * FROM vendor_platform_contract 
                WHERE vendor_id = @vendorId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@vendorId", vendorId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vendorContracts.Add(new VendorContract
                            {
                                VendorId = vendorId,
                                ContractId = reader.GetInt16(1),
                                Signature = reader.GetString(2),
                                SignedDate = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return vendorContracts;
        }
        public async Task <VendorContract> GetVendorContractAsync(Guid vendorId, short contractId)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                SELECT * FROM vendor_platform_contract 
                WHERE vendor_id = @vendorId AND contract_id = @contractId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@contractId", contractId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new VendorContract
                            {
                                VendorId = reader.GetGuid(0),
                                ContractId = reader.GetInt16(1),
                                Signature = reader.GetString(2),
                                SignedDate = reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<VendorContract>> GetVendorContractsAsync(short contractId)
        {
            var vendorContracts = new List<VendorContract>();
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                SELECT * FROM vendor_platform_contract 
                WHERE contract_id = @contractId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@contractId", contractId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vendorContracts.Add(new VendorContract
                            {
                                VendorId = reader.GetGuid(0),
                                ContractId = reader.GetInt16(1),
                                Signature = reader.GetString(2),
                                SignedDate = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return vendorContracts;
        }
        
        
        #endregion



        #region AddData

        public async Task AddVendorContractAsync(Guid vendorId, short contractId, string signature)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                INSERT INTO vendor_platform_contract (vendor_id, contract_id, signature, signed_date) 
                VALUES (@vendorId, @contractId, @signature, @signedDate)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@vendorId", vendorId);
                    command.Parameters.AddWithValue("@contractId", contractId);
                    command.Parameters.AddWithValue("@signature", signature);
                    command.Parameters.AddWithValue("@signedDate", DateTime.UtcNow);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }




        #endregion



        #region DeleteData

        public async Task DeleteVendorContractAsync(Guid vendorId, short contractId)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = @"
                DELETE FROM vendor_platform_contract 
                WHERE vendor_id = @vendorId AND contract_id = @contractId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@vendorId", vendorId);
                    command.Parameters.AddWithValue("@contractId", contractId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        #endregion
    }
}
