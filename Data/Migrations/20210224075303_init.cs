using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "drzave",
                columns: table => new
                {
                    drzavaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivDrzave = table.Column<string>(nullable: true),
                    zastavaDrzave = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drzave", x => x.drzavaID);
                });

            migrationBuilder.CreateTable(
                name: "garnitureDresova",
                columns: table => new
                {
                    garnituraDresovaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    opis = table.Column<int>(nullable: false),
                    lokacijaSlika = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_garnitureDresova", x => x.garnituraDresovaID);
                });

            migrationBuilder.CreateTable(
                name: "igracStatistika",
                columns: table => new
                {
                    igracStatistikaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brojNastupa = table.Column<int>(nullable: false),
                    odigraneMinute = table.Column<int>(nullable: false),
                    golovi = table.Column<int>(nullable: false),
                    asistencije = table.Column<int>(nullable: false),
                    zutiKartoni = table.Column<int>(nullable: false),
                    crveniKartoni = table.Column<int>(nullable: false),
                    zbirOcjena = table.Column<int>(nullable: false),
                    prosjecnaOcjena = table.Column<float>(nullable: false),
                    igracID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_igracStatistika", x => x.igracStatistikaID);
                });

            migrationBuilder.CreateTable(
                name: "stadioni",
                columns: table => new
                {
                    stadionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imeStadiona = table.Column<string>(nullable: true),
                    slikaStadiona = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stadioni", x => x.stadionID);
                });

            migrationBuilder.CreateTable(
                name: "trenerStatistika",
                columns: table => new
                {
                    trenerStatistikaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brojUtakmica = table.Column<int>(nullable: false),
                    brojPobjeda = table.Column<int>(nullable: false),
                    brojNerjesenih = table.Column<int>(nullable: false),
                    brojPoraza = table.Column<int>(nullable: false),
                    trenerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trenerStatistika", x => x.trenerStatistikaID);
                });

            migrationBuilder.CreateTable(
                name: "ugovori",
                columns: table => new
                {
                    ugovorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datumPocetka = table.Column<DateTime>(nullable: false),
                    datumZavrsetka = table.Column<DateTime>(nullable: false),
                    iznosPlate = table.Column<float>(nullable: false),
                    napomene = table.Column<string>(nullable: true),
                    duzinaUgovora = table.Column<int>(nullable: false),
                    igracID = table.Column<int>(nullable: true),
                    trenerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ugovori", x => x.ugovorID);
                });

            migrationBuilder.CreateTable(
                name: "vrsteTakmicenja",
                columns: table => new
                {
                    vrstaTakmicenjaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivVrsteTakmicenja = table.Column<string>(nullable: true),
                    logoTakmicenja = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vrsteTakmicenja", x => x.vrstaTakmicenjaID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gradovi",
                columns: table => new
                {
                    gradID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivGrada = table.Column<string>(nullable: true),
                    drzavaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gradovi", x => x.gradID);
                    table.ForeignKey(
                        name: "FK_gradovi_drzave_drzavaID",
                        column: x => x.drzavaID,
                        principalTable: "drzave",
                        principalColumn: "drzavaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ime = table.Column<string>(nullable: true),
                    prezime = table.Column<string>(nullable: true),
                    datumRodjenja = table.Column<DateTime>(nullable: false),
                    adresaPrebivalista = table.Column<string>(nullable: true),
                    telefon = table.Column<string>(nullable: true),
                    emailAdresa = table.Column<string>(nullable: true),
                    drzavljanstvoID = table.Column<int>(nullable: false),
                    gradRodjenjaID = table.Column<int>(nullable: false),
                    igracID = table.Column<int>(nullable: false),
                    trenerID = table.Column<int>(nullable: false),
                    isIgrac = table.Column<bool>(nullable: false),
                    isTrener = table.Column<bool>(nullable: false),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_drzave_drzavljanstvoID",
                        column: x => x.drzavljanstvoID,
                        principalTable: "drzave",
                        principalColumn: "drzavaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_gradovi_gradRodjenjaID",
                        column: x => x.gradRodjenjaID,
                        principalTable: "gradovi",
                        principalColumn: "gradID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "klubovi",
                columns: table => new
                {
                    klubID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivKluba = table.Column<string>(nullable: true),
                    grbKluba = table.Column<string>(nullable: true),
                    gradID = table.Column<int>(nullable: false),
                    stadionID = table.Column<int>(nullable: false),
                    domacaID = table.Column<int>(nullable: false),
                    gostujucaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_klubovi", x => x.klubID);
                    table.ForeignKey(
                        name: "FK_klubovi_garnitureDresova_domacaID",
                        column: x => x.domacaID,
                        principalTable: "garnitureDresova",
                        principalColumn: "garnituraDresovaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klubovi_garnitureDresova_gostujucaID",
                        column: x => x.gostujucaID,
                        principalTable: "garnitureDresova",
                        principalColumn: "garnituraDresovaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klubovi_gradovi_gradID",
                        column: x => x.gradID,
                        principalTable: "gradovi",
                        principalColumn: "gradID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klubovi_stadioni_stadionID",
                        column: x => x.stadionID,
                        principalTable: "stadioni",
                        principalColumn: "stadionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifikacije",
                columns: table => new
                {
                    notifikacijaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tekstNotifikacije = table.Column<string>(nullable: true),
                    datumNotifikacije = table.Column<DateTime>(nullable: false),
                    statusNotifikacije = table.Column<int>(nullable: false),
                    tipNotifikacije = table.Column<int>(nullable: false),
                    poruka = table.Column<bool>(nullable: false),
                    posiljaoc = table.Column<string>(nullable: true),
                    KorisnikId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifikacije", x => x.notifikacijaID);
                    table.ForeignKey(
                        name: "FK_notifikacije_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "treneri",
                columns: table => new
                {
                    trenerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imePrezime = table.Column<string>(nullable: true),
                    prvaZvanicnaUtakmica = table.Column<DateTime>(nullable: false),
                    godineIskustva = table.Column<int>(nullable: false),
                    datumPristupaKlubu = table.Column<DateTime>(nullable: false),
                    slika = table.Column<string>(nullable: true),
                    dosadasnjiKlubovi = table.Column<string>(nullable: true),
                    tranfermarktAcc = table.Column<string>(nullable: true),
                    trzisnaVrijednost = table.Column<float>(nullable: false),
                    trenerStatistikaID = table.Column<int>(nullable: false),
                    ugovorID = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    korisnikID = table.Column<string>(nullable: true),
                    preferiranaFormacija = table.Column<int>(nullable: false),
                    trenerskaLicenca = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treneri", x => x.trenerID);
                    table.ForeignKey(
                        name: "FK_treneri_AspNetUsers_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_treneri_trenerStatistika_trenerStatistikaID",
                        column: x => x.trenerStatistikaID,
                        principalTable: "trenerStatistika",
                        principalColumn: "trenerStatistikaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_treneri_ugovori_ugovorID",
                        column: x => x.ugovorID,
                        principalTable: "ugovori",
                        principalColumn: "ugovorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "zahtjevi",
                columns: table => new
                {
                    zahtjevID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    svrha = table.Column<int>(nullable: false),
                    tipZahtjeva = table.Column<int>(nullable: false),
                    statusZahtjeva = table.Column<int>(nullable: false),
                    prioritet = table.Column<int>(nullable: false),
                    datum = table.Column<DateTime>(nullable: false),
                    odgovor = table.Column<string>(nullable: true),
                    korisnikID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zahtjevi", x => x.zahtjevID);
                    table.ForeignKey(
                        name: "FK_zahtjevi_AspNetUsers_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "treninzi",
                columns: table => new
                {
                    treningID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datumOdrzavanja = table.Column<DateTime>(nullable: false),
                    satnica = table.Column<string>(nullable: true),
                    lokacija = table.Column<int>(nullable: false),
                    fokusTreninga = table.Column<string>(nullable: true),
                    statusTreninga = table.Column<int>(nullable: false),
                    zabiljezioID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treninzi", x => x.treningID);
                    table.ForeignKey(
                        name: "FK_treninzi_treneri_zabiljezioID",
                        column: x => x.zabiljezioID,
                        principalTable: "treneri",
                        principalColumn: "trenerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "igraci",
                columns: table => new
                {
                    igracID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    visina = table.Column<float>(nullable: false),
                    tezina = table.Column<int>(nullable: false),
                    brojDresa = table.Column<int>(nullable: false),
                    trzisnaVrijednost = table.Column<float>(nullable: false),
                    reprezentativac = table.Column<bool>(nullable: false),
                    slika = table.Column<string>(nullable: true),
                    datumPristupaKlubu = table.Column<DateTime>(nullable: false),
                    dosadasnjiKlubovi = table.Column<string>(nullable: true),
                    trensferMarktAcc = table.Column<string>(nullable: true),
                    godine = table.Column<int>(nullable: false),
                    pozicija = table.Column<int>(nullable: false),
                    boljaNoga = table.Column<int>(nullable: false),
                    igracStatistikaID = table.Column<int>(nullable: false),
                    igracSkillsID = table.Column<int>(nullable: false),
                    ugovorID = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    korisnikID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_igraci", x => x.igracID);
                    table.ForeignKey(
                        name: "FK_igraci_igracStatistika_igracStatistikaID",
                        column: x => x.igracStatistikaID,
                        principalTable: "igracStatistika",
                        principalColumn: "igracStatistikaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_igraci_AspNetUsers_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_igraci_ugovori_ugovorID",
                        column: x => x.ugovorID,
                        principalTable: "ugovori",
                        principalColumn: "ugovorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "izmjena",
                columns: table => new
                {
                    izmjenaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    igracOutID = table.Column<int>(nullable: false),
                    igracInID = table.Column<int>(nullable: false),
                    minuta = table.Column<int>(nullable: false),
                    razlog = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_izmjena", x => x.izmjenaID);
                    table.ForeignKey(
                        name: "FK_izmjena_igraci_igracInID",
                        column: x => x.igracInID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_izmjena_igraci_igracOutID",
                        column: x => x.igracOutID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "klupa",
                columns: table => new
                {
                    klupaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    utakmicaID = table.Column<int>(nullable: true),
                    golmanKlupaID = table.Column<int>(nullable: false),
                    odbranaKlupaID = table.Column<int>(nullable: false),
                    bekKlupaID = table.Column<int>(nullable: false),
                    veznjakKlupaID = table.Column<int>(nullable: false),
                    kriloKlupaID = table.Column<int>(nullable: false),
                    napadacKlupaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_klupa", x => x.klupaID);
                    table.ForeignKey(
                        name: "FK_klupa_igraci_bekKlupaID",
                        column: x => x.bekKlupaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klupa_igraci_golmanKlupaID",
                        column: x => x.golmanKlupaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klupa_igraci_kriloKlupaID",
                        column: x => x.kriloKlupaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klupa_igraci_napadacKlupaID",
                        column: x => x.napadacKlupaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klupa_igraci_odbranaKlupaID",
                        column: x => x.odbranaKlupaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_klupa_igraci_veznjakKlupaID",
                        column: x => x.veznjakKlupaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prvaPostava",
                columns: table => new
                {
                    prvaPostavaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    utakmicaID = table.Column<int>(nullable: true),
                    golmanID = table.Column<int>(nullable: false),
                    lijeviBekID = table.Column<int>(nullable: false),
                    prviStoperID = table.Column<int>(nullable: false),
                    drugiStoperID = table.Column<int>(nullable: false),
                    desniBekID = table.Column<int>(nullable: false),
                    prviZadnjiVezniID = table.Column<int>(nullable: false),
                    drugiZadnjiVezniID = table.Column<int>(nullable: false),
                    prednjiVezniID = table.Column<int>(nullable: false),
                    lijevoKriloID = table.Column<int>(nullable: false),
                    desnoKriloID = table.Column<int>(nullable: false),
                    napadacID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prvaPostava", x => x.prvaPostavaID);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_desniBekID",
                        column: x => x.desniBekID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_desnoKriloID",
                        column: x => x.desnoKriloID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_drugiStoperID",
                        column: x => x.drugiStoperID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_drugiZadnjiVezniID",
                        column: x => x.drugiZadnjiVezniID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_golmanID",
                        column: x => x.golmanID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_lijeviBekID",
                        column: x => x.lijeviBekID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_lijevoKriloID",
                        column: x => x.lijevoKriloID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_napadacID",
                        column: x => x.napadacID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_prednjiVezniID",
                        column: x => x.prednjiVezniID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_prviStoperID",
                        column: x => x.prviStoperID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prvaPostava_igraci_prviZadnjiVezniID",
                        column: x => x.prviZadnjiVezniID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "skillsStavke",
                columns: table => new
                {
                    skillsStavkeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kontrolaLopte = table.Column<float>(nullable: false),
                    kontrolaLopteZbir = table.Column<int>(nullable: false),
                    dribljanje = table.Column<float>(nullable: false),
                    dribljanjeZbir = table.Column<int>(nullable: false),
                    dodavanje = table.Column<float>(nullable: false),
                    dodavanjeZbir = table.Column<int>(nullable: false),
                    kretanje = table.Column<float>(nullable: false),
                    kretanjeZbir = table.Column<int>(nullable: false),
                    brzina = table.Column<float>(nullable: false),
                    brzinaZbir = table.Column<int>(nullable: false),
                    sut = table.Column<float>(nullable: false),
                    sutZbir = table.Column<int>(nullable: false),
                    snaga = table.Column<float>(nullable: false),
                    snagaZbir = table.Column<int>(nullable: false),
                    markiranje = table.Column<float>(nullable: false),
                    markiranjeZbir = table.Column<int>(nullable: false),
                    klizeciStart = table.Column<float>(nullable: false),
                    klizeciStartZbir = table.Column<int>(nullable: false),
                    skok = table.Column<float>(nullable: false),
                    skokZbir = table.Column<int>(nullable: false),
                    odbrana = table.Column<float>(nullable: false),
                    odbranaZbir = table.Column<int>(nullable: false),
                    zbirOcjena = table.Column<float>(nullable: false),
                    prosjekOcjena = table.Column<float>(nullable: false),
                    igracID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skillsStavke", x => x.skillsStavkeID);
                    table.ForeignKey(
                        name: "FK_skillsStavke_igraci_igracID",
                        column: x => x.igracID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "utakmice",
                columns: table => new
                {
                    utakmicaID = table.Column<int>(nullable: false),
                    datumOdigravanja = table.Column<DateTime>(nullable: false),
                    satnica = table.Column<string>(nullable: true),
                    sudija = table.Column<string>(nullable: true),
                    stadionID = table.Column<int>(nullable: false),
                    napomene = table.Column<string>(nullable: true),
                    brojUlaznica = table.Column<int>(nullable: false),
                    cijenaUlaznice = table.Column<float>(nullable: false),
                    vrstaTakmicenjaID = table.Column<int>(nullable: false),
                    kapitenID = table.Column<int>(nullable: false),
                    trenerID = table.Column<int>(nullable: false),
                    domacinID = table.Column<int>(nullable: false),
                    gostID = table.Column<int>(nullable: false),
                    garnituraDresaID = table.Column<int>(nullable: false),
                    vrstaUtakmice = table.Column<int>(nullable: false),
                    statusUtakmice = table.Column<int>(nullable: false),
                    PredvidjenoVrijeme = table.Column<int>(nullable: false),
                    prvaPostavaID = table.Column<int>(nullable: false),
                    klupaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utakmice", x => x.utakmicaID);
                    table.ForeignKey(
                        name: "FK_utakmice_klubovi_domacinID",
                        column: x => x.domacinID,
                        principalTable: "klubovi",
                        principalColumn: "klubID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmice_garnitureDresova_garnituraDresaID",
                        column: x => x.garnituraDresaID,
                        principalTable: "garnitureDresova",
                        principalColumn: "garnituraDresovaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmice_klubovi_gostID",
                        column: x => x.gostID,
                        principalTable: "klubovi",
                        principalColumn: "klubID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmice_igraci_kapitenID",
                        column: x => x.kapitenID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmice_stadioni_stadionID",
                        column: x => x.stadionID,
                        principalTable: "stadioni",
                        principalColumn: "stadionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmice_treneri_trenerID",
                        column: x => x.trenerID,
                        principalTable: "treneri",
                        principalColumn: "trenerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmice_klupa_utakmicaID",
                        column: x => x.utakmicaID,
                        principalTable: "klupa",
                        principalColumn: "klupaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_utakmice_prvaPostava_utakmicaID",
                        column: x => x.utakmicaID,
                        principalTable: "prvaPostava",
                        principalColumn: "prvaPostavaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_utakmice_vrsteTakmicenja_vrstaTakmicenjaID",
                        column: x => x.vrstaTakmicenjaID,
                        principalTable: "vrsteTakmicenja",
                        principalColumn: "vrstaTakmicenjaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "igracSkills",
                columns: table => new
                {
                    igracSkillsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skillsStavkeID = table.Column<int>(nullable: true),
                    igracID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_igracSkills", x => x.igracSkillsID);
                    table.ForeignKey(
                        name: "FK_igracSkills_skillsStavke_skillsStavkeID",
                        column: x => x.skillsStavkeID,
                        principalTable: "skillsStavke",
                        principalColumn: "skillsStavkeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "izvjestaji",
                columns: table => new
                {
                    izvještajID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    goloviGostiju = table.Column<int>(nullable: false),
                    goloviDomacih = table.Column<int>(nullable: false),
                    rezultat = table.Column<int>(nullable: false),
                    igracUtakmicaID = table.Column<int>(nullable: false),
                    delegatUtakmice = table.Column<string>(nullable: true),
                    utakmicaID = table.Column<int>(nullable: false),
                    trenerID = table.Column<int>(nullable: false),
                    brojGledalaca = table.Column<int>(nullable: false),
                    vrijeme = table.Column<int>(nullable: false),
                    datumKreiranja = table.Column<DateTime>(nullable: false),
                    zapisnik = table.Column<string>(nullable: true),
                    slikaSaUtakmice = table.Column<string>(nullable: true),
                    youtubeVideoID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_izvjestaji", x => x.izvještajID);
                    table.ForeignKey(
                        name: "FK_izvjestaji_igraci_igracUtakmicaID",
                        column: x => x.igracUtakmicaID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_izvjestaji_treneri_trenerID",
                        column: x => x.trenerID,
                        principalTable: "treneri",
                        principalColumn: "trenerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_izvjestaji_utakmice_utakmicaID",
                        column: x => x.utakmicaID,
                        principalTable: "utakmice",
                        principalColumn: "utakmicaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "nastupi",
                columns: table => new
                {
                    utakmicaNastupiID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    igracID = table.Column<int>(nullable: false),
                    utakmicaID = table.Column<int>(nullable: false),
                    minute = table.Column<int>(nullable: false),
                    golovi = table.Column<int>(nullable: false),
                    asistencije = table.Column<int>(nullable: false),
                    zutiKartoni = table.Column<int>(nullable: false),
                    crveniKartoni = table.Column<int>(nullable: false),
                    ocjena = table.Column<int>(nullable: false),
                    komentar = table.Column<string>(nullable: true),
                    izvještajID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nastupi", x => x.utakmicaNastupiID);
                    table.ForeignKey(
                        name: "FK_nastupi_igraci_igracID",
                        column: x => x.igracID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nastupi_izvjestaji_izvještajID",
                        column: x => x.izvještajID,
                        principalTable: "izvjestaji",
                        principalColumn: "izvještajID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nastupi_utakmice_utakmicaID",
                        column: x => x.utakmicaID,
                        principalTable: "utakmice",
                        principalColumn: "utakmicaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ocjene",
                columns: table => new
                {
                    utakmicaOcjeneID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    igracID = table.Column<int>(nullable: false),
                    utakmicaID = table.Column<int>(nullable: false),
                    kontrolaLopte = table.Column<int>(nullable: false),
                    dribljanje = table.Column<int>(nullable: false),
                    dodavanje = table.Column<int>(nullable: false),
                    kretanje = table.Column<int>(nullable: false),
                    brzina = table.Column<int>(nullable: false),
                    sut = table.Column<int>(nullable: false),
                    snaga = table.Column<int>(nullable: false),
                    markiranje = table.Column<int>(nullable: false),
                    klizeciStart = table.Column<int>(nullable: false),
                    skok = table.Column<int>(nullable: false),
                    odbrana = table.Column<int>(nullable: false),
                    prosjecnaOcjena = table.Column<float>(nullable: false),
                    izvještajID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ocjene", x => x.utakmicaOcjeneID);
                    table.ForeignKey(
                        name: "FK_ocjene_igraci_igracID",
                        column: x => x.igracID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ocjene_izvjestaji_izvještajID",
                        column: x => x.izvještajID,
                        principalTable: "izvjestaji",
                        principalColumn: "izvještajID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ocjene_utakmice_utakmicaID",
                        column: x => x.utakmicaID,
                        principalTable: "utakmice",
                        principalColumn: "utakmicaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "utakmicaIzmjena",
                columns: table => new
                {
                    utakmicaIzmjenaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    utakmicaID = table.Column<int>(nullable: false),
                    izmjenaID = table.Column<int>(nullable: false),
                    izvještajID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utakmicaIzmjena", x => x.utakmicaIzmjenaID);
                    table.ForeignKey(
                        name: "FK_utakmicaIzmjena_izmjena_izmjenaID",
                        column: x => x.izmjenaID,
                        principalTable: "izmjena",
                        principalColumn: "izmjenaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmicaIzmjena_izvjestaji_izvještajID",
                        column: x => x.izvještajID,
                        principalTable: "izvjestaji",
                        principalColumn: "izvještajID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utakmicaIzmjena_utakmice_utakmicaID",
                        column: x => x.utakmicaID,
                        principalTable: "utakmice",
                        principalColumn: "utakmicaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_drzavljanstvoID",
                table: "AspNetUsers",
                column: "drzavljanstvoID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_gradRodjenjaID",
                table: "AspNetUsers",
                column: "gradRodjenjaID");

            migrationBuilder.CreateIndex(
                name: "IX_gradovi_drzavaID",
                table: "gradovi",
                column: "drzavaID");

            migrationBuilder.CreateIndex(
                name: "IX_igraci_igracSkillsID",
                table: "igraci",
                column: "igracSkillsID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_igraci_igracStatistikaID",
                table: "igraci",
                column: "igracStatistikaID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_igraci_korisnikID",
                table: "igraci",
                column: "korisnikID",
                unique: true,
                filter: "[korisnikID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_igraci_ugovorID",
                table: "igraci",
                column: "ugovorID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_igracSkills_skillsStavkeID",
                table: "igracSkills",
                column: "skillsStavkeID");

            migrationBuilder.CreateIndex(
                name: "IX_izmjena_igracInID",
                table: "izmjena",
                column: "igracInID");

            migrationBuilder.CreateIndex(
                name: "IX_izmjena_igracOutID",
                table: "izmjena",
                column: "igracOutID");

            migrationBuilder.CreateIndex(
                name: "IX_izvjestaji_igracUtakmicaID",
                table: "izvjestaji",
                column: "igracUtakmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_izvjestaji_trenerID",
                table: "izvjestaji",
                column: "trenerID");

            migrationBuilder.CreateIndex(
                name: "IX_izvjestaji_utakmicaID",
                table: "izvjestaji",
                column: "utakmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_klubovi_domacaID",
                table: "klubovi",
                column: "domacaID");

            migrationBuilder.CreateIndex(
                name: "IX_klubovi_gostujucaID",
                table: "klubovi",
                column: "gostujucaID");

            migrationBuilder.CreateIndex(
                name: "IX_klubovi_gradID",
                table: "klubovi",
                column: "gradID");

            migrationBuilder.CreateIndex(
                name: "IX_klubovi_stadionID",
                table: "klubovi",
                column: "stadionID");

            migrationBuilder.CreateIndex(
                name: "IX_klupa_bekKlupaID",
                table: "klupa",
                column: "bekKlupaID");

            migrationBuilder.CreateIndex(
                name: "IX_klupa_golmanKlupaID",
                table: "klupa",
                column: "golmanKlupaID");

            migrationBuilder.CreateIndex(
                name: "IX_klupa_kriloKlupaID",
                table: "klupa",
                column: "kriloKlupaID");

            migrationBuilder.CreateIndex(
                name: "IX_klupa_napadacKlupaID",
                table: "klupa",
                column: "napadacKlupaID");

            migrationBuilder.CreateIndex(
                name: "IX_klupa_odbranaKlupaID",
                table: "klupa",
                column: "odbranaKlupaID");

            migrationBuilder.CreateIndex(
                name: "IX_klupa_veznjakKlupaID",
                table: "klupa",
                column: "veznjakKlupaID");

            migrationBuilder.CreateIndex(
                name: "IX_nastupi_igracID",
                table: "nastupi",
                column: "igracID");

            migrationBuilder.CreateIndex(
                name: "IX_nastupi_izvještajID",
                table: "nastupi",
                column: "izvještajID");

            migrationBuilder.CreateIndex(
                name: "IX_nastupi_utakmicaID",
                table: "nastupi",
                column: "utakmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_notifikacije_KorisnikId",
                table: "notifikacije",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_ocjene_igracID",
                table: "ocjene",
                column: "igracID");

            migrationBuilder.CreateIndex(
                name: "IX_ocjene_izvještajID",
                table: "ocjene",
                column: "izvještajID");

            migrationBuilder.CreateIndex(
                name: "IX_ocjene_utakmicaID",
                table: "ocjene",
                column: "utakmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_desniBekID",
                table: "prvaPostava",
                column: "desniBekID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_desnoKriloID",
                table: "prvaPostava",
                column: "desnoKriloID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_drugiStoperID",
                table: "prvaPostava",
                column: "drugiStoperID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_drugiZadnjiVezniID",
                table: "prvaPostava",
                column: "drugiZadnjiVezniID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_golmanID",
                table: "prvaPostava",
                column: "golmanID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_lijeviBekID",
                table: "prvaPostava",
                column: "lijeviBekID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_lijevoKriloID",
                table: "prvaPostava",
                column: "lijevoKriloID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_napadacID",
                table: "prvaPostava",
                column: "napadacID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_prednjiVezniID",
                table: "prvaPostava",
                column: "prednjiVezniID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_prviStoperID",
                table: "prvaPostava",
                column: "prviStoperID");

            migrationBuilder.CreateIndex(
                name: "IX_prvaPostava_prviZadnjiVezniID",
                table: "prvaPostava",
                column: "prviZadnjiVezniID");

            migrationBuilder.CreateIndex(
                name: "IX_skillsStavke_igracID",
                table: "skillsStavke",
                column: "igracID");

            migrationBuilder.CreateIndex(
                name: "IX_treneri_korisnikID",
                table: "treneri",
                column: "korisnikID",
                unique: true,
                filter: "[korisnikID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_treneri_trenerStatistikaID",
                table: "treneri",
                column: "trenerStatistikaID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_treneri_ugovorID",
                table: "treneri",
                column: "ugovorID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_treninzi_zabiljezioID",
                table: "treninzi",
                column: "zabiljezioID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmicaIzmjena_izmjenaID",
                table: "utakmicaIzmjena",
                column: "izmjenaID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmicaIzmjena_izvještajID",
                table: "utakmicaIzmjena",
                column: "izvještajID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmicaIzmjena_utakmicaID",
                table: "utakmicaIzmjena",
                column: "utakmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_domacinID",
                table: "utakmice",
                column: "domacinID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_garnituraDresaID",
                table: "utakmice",
                column: "garnituraDresaID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_gostID",
                table: "utakmice",
                column: "gostID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_kapitenID",
                table: "utakmice",
                column: "kapitenID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_stadionID",
                table: "utakmice",
                column: "stadionID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_trenerID",
                table: "utakmice",
                column: "trenerID");

            migrationBuilder.CreateIndex(
                name: "IX_utakmice_vrstaTakmicenjaID",
                table: "utakmice",
                column: "vrstaTakmicenjaID");

            migrationBuilder.CreateIndex(
                name: "IX_zahtjevi_korisnikID",
                table: "zahtjevi",
                column: "korisnikID");

            migrationBuilder.AddForeignKey(
                name: "FK_igraci_igracSkills_igracSkillsID",
                table: "igraci",
                column: "igracSkillsID",
                principalTable: "igracSkills",
                principalColumn: "igracSkillsID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_igraci_AspNetUsers_korisnikID",
                table: "igraci");

            migrationBuilder.DropForeignKey(
                name: "FK_igraci_igracSkills_igracSkillsID",
                table: "igraci");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "nastupi");

            migrationBuilder.DropTable(
                name: "notifikacije");

            migrationBuilder.DropTable(
                name: "ocjene");

            migrationBuilder.DropTable(
                name: "treninzi");

            migrationBuilder.DropTable(
                name: "utakmicaIzmjena");

            migrationBuilder.DropTable(
                name: "zahtjevi");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "izmjena");

            migrationBuilder.DropTable(
                name: "izvjestaji");

            migrationBuilder.DropTable(
                name: "utakmice");

            migrationBuilder.DropTable(
                name: "klubovi");

            migrationBuilder.DropTable(
                name: "treneri");

            migrationBuilder.DropTable(
                name: "klupa");

            migrationBuilder.DropTable(
                name: "prvaPostava");

            migrationBuilder.DropTable(
                name: "vrsteTakmicenja");

            migrationBuilder.DropTable(
                name: "garnitureDresova");

            migrationBuilder.DropTable(
                name: "stadioni");

            migrationBuilder.DropTable(
                name: "trenerStatistika");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "gradovi");

            migrationBuilder.DropTable(
                name: "drzave");

            migrationBuilder.DropTable(
                name: "igracSkills");

            migrationBuilder.DropTable(
                name: "skillsStavke");

            migrationBuilder.DropTable(
                name: "igraci");

            migrationBuilder.DropTable(
                name: "igracStatistika");

            migrationBuilder.DropTable(
                name: "ugovori");
        }
    }
}
