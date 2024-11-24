﻿// <auto-generated />
using System;
using HW_6_ChatApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HW_6_ChatApp.Migrations
{
    [DbContext(typeof(ContextDB))]
    partial class ContextDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HW_6_ChatApp.Model.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateMessage")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FromUserId")
                        .HasColumnType("integer")
                        .HasColumnName("from_user_id");

                    b.Property<bool>("IsReceived")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<int?>("ToUserId")
                        .HasColumnType("integer")
                        .HasColumnName("to_user_id");

                    b.HasKey("Id")
                        .HasName("message_pkey");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("HW_6_ChatApp.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("HW_6_ChatApp.Model.Message", b =>
                {
                    b.HasOne("HW_6_ChatApp.Model.User", "FromUser")
                        .WithMany("FromMessages")
                        .HasForeignKey("FromUserId")
                        .HasConstraintName("messages_from_user_id_fkey");

                    b.HasOne("HW_6_ChatApp.Model.User", "ToUser")
                        .WithMany("ToMessages")
                        .HasForeignKey("ToUserId")
                        .HasConstraintName("messages_to_user_id_fkey");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("HW_6_ChatApp.Model.User", b =>
                {
                    b.Navigation("FromMessages");

                    b.Navigation("ToMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
