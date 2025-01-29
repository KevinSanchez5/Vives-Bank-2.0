﻿// <auto-generated />
using System;
using Banco_VivesBank.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Banco_VivesBank.Migrations
{
    [DbContext(typeof(GeneralDbContext))]
    partial class GeneralDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.ClienteEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FotoDni")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FotoPerfil")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Clientes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Apellidos = "Picapiedra",
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6293),
                            Dni = "12345678Z",
                            Email = "pedro.picapiedra@gmail.com",
                            FotoDni = "https://example.com/fotoDniPedro.jpg",
                            FotoPerfil = "https://example.com/fotoPerfilPedro.jpg",
                            Guid = "GbJtJkggUOM",
                            IsDeleted = false,
                            Nombre = "Pedro",
                            Telefono = "612345678",
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6294),
                            UserId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            Apellidos = "Martinez",
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6368),
                            Dni = "21240915R",
                            Email = "ana.martinez@gmail.com",
                            FotoDni = "https://example.com/fotoDniAna.jpg",
                            FotoPerfil = "https://example.com/fotoPerfilAna.jpg",
                            Guid = "JdHsgzoHlrb",
                            IsDeleted = false,
                            Nombre = "Ana",
                            Telefono = "623456789",
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6368),
                            UserId = 2L
                        });
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.CuentaEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ClienteId")
                        .HasColumnType("bigint")
                        .HasColumnName("cliente_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<long>("ProductoId")
                        .HasColumnType("bigint")
                        .HasColumnName("producto_id");

                    b.Property<double>("Saldo")
                        .HasColumnType("double precision");

                    b.Property<long?>("TarjetaId")
                        .HasColumnType("bigint")
                        .HasColumnName("tarjeta_id");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ProductoId");

                    b.HasIndex("TarjetaId")
                        .IsUnique();

                    b.ToTable("Cuentas");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ClienteId = 1L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(24),
                            Guid = "VWt47641yDI",
                            Iban = "ES7730046576085345979538",
                            IsDeleted = false,
                            ProductoId = 1L,
                            Saldo = 5000.0,
                            TarjetaId = 1L,
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(24)
                        },
                        new
                        {
                            Id = 2L,
                            ClienteId = 2L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(41),
                            Guid = "oVUzRuFwMlf",
                            Iban = "ES2114656261103572788444",
                            IsDeleted = false,
                            ProductoId = 2L,
                            Saldo = 7000.0,
                            TarjetaId = 2L,
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(42)
                        });
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.ProductoEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<double>("Tae")
                        .HasColumnType("double precision");

                    b.Property<string>("TipoProducto")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Productos");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6802),
                            Descripcion = "Producto para cuenta bancaria de ahorros",
                            Guid = "yFlOirSXTaL",
                            IsDeleted = false,
                            Nombre = "Cuenta de ahorros",
                            Tae = 2.5,
                            TipoProducto = "cuentaAhorros",
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6803)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6810),
                            Descripcion = "Producto para cuenta bancaria corriente",
                            Guid = "dEmAjXpMTmy",
                            IsDeleted = false,
                            Nombre = "Cuenta corriente",
                            Tae = 1.5,
                            TipoProducto = "cuentaCorriente",
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(6811)
                        });
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.TarjetaEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Cvv")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FechaVencimiento")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<double>("LimiteDiario")
                        .HasColumnType("double precision");

                    b.Property<double>("LimiteMensual")
                        .HasColumnType("double precision");

                    b.Property<double>("LimiteSemanal")
                        .HasColumnType("double precision");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Tarjetas");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(581),
                            Cvv = "298",
                            FechaVencimiento = "04/27",
                            Guid = "HGyMfulgniP",
                            IsDeleted = false,
                            LimiteDiario = 500.0,
                            LimiteMensual = 10000.0,
                            LimiteSemanal = 2500.0,
                            Numero = "0606579225434779",
                            Pin = "1234",
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(581)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(620),
                            Cvv = "425",
                            FechaVencimiento = "06/26",
                            Guid = "W71vOHuFzS4",
                            IsDeleted = false,
                            LimiteDiario = 100.0,
                            LimiteMensual = 2500.0,
                            LimiteSemanal = 1500.0,
                            Numero = "0751528101703123",
                            Pin = "4321",
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 535, DateTimeKind.Utc).AddTicks(621)
                        });
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(4086),
                            Guid = "vz2AWLK8YPS",
                            IsDeleted = false,
                            Password = "$2a$11$H8eSJTQ0cZjHNmozhjcW6ep/5jUQDnt7FrUmgbNKxww897iMniVfe",
                            Role = 0,
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(4086),
                            Username = "pedrito"
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(4113),
                            Guid = "6t8gVeTQt2w",
                            IsDeleted = false,
                            Password = "$2a$11$H8eSJTQ0cZjHNmozhjcW6ep/5jUQDnt7FrUmgbNKxww897iMniVfe",
                            Role = 0,
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(4113),
                            Username = "anita"
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(4166),
                            Guid = "u6b6NDClz5o",
                            IsDeleted = false,
                            Password = "$2a$11$H8eSJTQ0cZjHNmozhjcW6ep/5jUQDnt7FrUmgbNKxww897iMniVfe",
                            Role = 1,
                            UpdatedAt = new DateTime(2025, 1, 29, 11, 9, 33, 534, DateTimeKind.Utc).AddTicks(4166),
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.ClienteEntity", b =>
                {
                    b.HasOne("Banco_VivesBank.Database.Entities.UserEntity", "User")
                        .WithOne()
                        .HasForeignKey("Banco_VivesBank.Database.Entities.ClienteEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Banco_VivesBank.Cliente.Models.Direccion", "Direccion", b1 =>
                        {
                            b1.Property<long>("ClienteEntityId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Calle")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("Calle")
                                .HasAnnotation("Relational:JsonPropertyName", "calle");

                            b1.Property<string>("CodigoPostal")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("CodigoPostal")
                                .HasAnnotation("Relational:JsonPropertyName", "codigoPostal");

                            b1.Property<string>("Letra")
                                .IsRequired()
                                .HasMaxLength(2)
                                .HasColumnType("character varying(2)")
                                .HasColumnName("Letra")
                                .HasAnnotation("Relational:JsonPropertyName", "letra");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("character varying(5)")
                                .HasColumnName("Numero")
                                .HasAnnotation("Relational:JsonPropertyName", "numero");

                            b1.Property<string>("Piso")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("character varying(3)")
                                .HasColumnName("Piso")
                                .HasAnnotation("Relational:JsonPropertyName", "piso");

                            b1.HasKey("ClienteEntityId");

                            b1.ToTable("Clientes");

                            b1.WithOwner()
                                .HasForeignKey("ClienteEntityId");

                            b1.HasData(
                                new
                                {
                                    ClienteEntityId = 1L,
                                    Calle = "Calle Uno",
                                    CodigoPostal = "28001",
                                    Letra = "A",
                                    Numero = "123",
                                    Piso = "1"
                                },
                                new
                                {
                                    ClienteEntityId = 2L,
                                    Calle = "Calle Dos",
                                    CodigoPostal = "28002",
                                    Letra = "B",
                                    Numero = "456",
                                    Piso = "2"
                                });
                        });

                    b.Navigation("Direccion")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.CuentaEntity", b =>
                {
                    b.HasOne("Banco_VivesBank.Database.Entities.ClienteEntity", "Cliente")
                        .WithMany("Cuentas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Banco_VivesBank.Database.Entities.ProductoEntity", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Banco_VivesBank.Database.Entities.TarjetaEntity", "Tarjeta")
                        .WithOne()
                        .HasForeignKey("Banco_VivesBank.Database.Entities.CuentaEntity", "TarjetaId");

                    b.Navigation("Cliente");

                    b.Navigation("Producto");

                    b.Navigation("Tarjeta");
                });

            modelBuilder.Entity("Banco_VivesBank.Database.Entities.ClienteEntity", b =>
                {
                    b.Navigation("Cuentas");
                });
#pragma warning restore 612, 618
        }
    }
}
