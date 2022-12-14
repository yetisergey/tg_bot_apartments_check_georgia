// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Storage.Bot;

#nullable disable

namespace Storage.Bot.Migrations
{
    [DbContext(typeof(BotContext))]
    [Migration("20221014112604_addedcitymapc")]
    partial class addedcitymapc
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Storage.Bot.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("Cities")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Districts")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullRegions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("MapC")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameGe")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Regions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Cities = "1996871",
                            Districts = "62176122.319380261.58416723.2953929439.58420997.152297954.61645269.6273968347.58416582.58416672.58377946",
                            FullRegions = "687578743",
                            GID = "1996871",
                            IsDefault = true,
                            MapC = "41.73188365,44.8368762993663",
                            Name = "Tbilisi",
                            NameGe = "თბილისი",
                            Regions = "687578743"
                        },
                        new
                        {
                            Id = 2,
                            Cities = "8742159",
                            Districts = "776481390.776472116.776471185.777654897.776734274.776998491.776460995.776458944.776463102.776465448",
                            FullRegions = "",
                            GID = "8742159",
                            IsDefault = false,
                            MapC = "41.73188365,44.8368762993663",
                            Name = "Batumi",
                            NameGe = "ბათუმი",
                            Regions = ""
                        });
                });

            modelBuilder.Entity("Storage.Bot.Models.Filter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<decimal>("DownPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("UpPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Filters");
                });

            modelBuilder.Entity("Storage.Bot.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Nickname" }, "Nickname")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Storage.Bot.Models.Filter", b =>
                {
                    b.HasOne("Storage.Bot.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Storage.Bot.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("Storage.Bot.Models.Filter", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
