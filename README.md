# MATF Društvena Mreža

## 📖 Opis Projekta

MATF Društvena Mreža je moderna veb aplikacija dizajnirana za studente i nastavnike Matematičkog fakulteta Univerziteta u Beogradu. Aplikacija omogućava korisnicima da se povezuju, dele sadržaj, razmenjuju poruke i grade svoju akademsku mrežu u realnom vremenu.

## 🏗️ Arhitektura Sistema

Aplikacija je izgrađena koristeći **mikroservisnu arhitekturu** sa sledećim glavnim komponentama:

### Backend Mikroservisi
- **Identity Service** - Autentifikacija i upravljanje korisnicima
- **Post Service** - Upravljanje objavama, komentarima i lajkovima
- **Relations Service** - Upravljanje prijateljstvima i odnosima između korisnika
- **Chat Service** - Real-time chat funkcionalnost

### Frontend
- **Vue.js 3 Web Aplikacija** - Moderna SPA aplikacija sa Vuetify UI bibliotekom

### Infrastruktura
- **RabbitMQ** - Message broker za komunikaciju između servisa
- **SignalR** - Real-time komunikacija za chat funkcionalnost
- **Neo4j** - Graph baza podataka za upravljanje odnosima
- **SQL Server** - Relaciona baza podataka za ostale servise

## 🛠️ Tehnološki Stek

### Backend (.NET 8)
- **ASP.NET Core 8** - Web API framework
- **Entity Framework Core** - ORM za pristup bazi podataka
- **MediatR** - CQRS pattern implementacija
- **AutoMapper** - Object-to-object mapping
- **MassTransit** - Distributed application framework
- **SignalR** - Real-time web functionality
- **JWT Bearer Authentication** - Token-based authentication
- **Swagger/OpenAPI** - API dokumentacija

### Frontend (Vue.js 3)
- **Vue 3** - Progressive JavaScript framework
- **TypeScript** - Type-safe JavaScript
- **Vuetify 3** - Material Design component library
- **Vue Router** - Client-side routing
- **Pinia** - State management
- **Axios** - HTTP client
- **Vite** - Build tool and dev server

### Baze Podataka
- **SQL Server** - Glavni RDBMS
- **Neo4j** - Graph database za relationship management

### DevOps & Infrastruktura
- **Docker** - Kontejnerizacija
- **RabbitMQ** - Message queuing
- **Custom Logger Middleware** - Strukturirano logovanje

## ✨ Funkcionalnosti

### 👤 Upravljanje Korisnicima
- **Registracija i prijava** korisnika
- **JWT token autentifikacija** sa refresh tokenima
- **Profili korisnika** sa mogućnošću učitavanja profilnih slika
- **Pretraga korisnika** po korisničkom imenu

### 👥 Sistem Prijateljstava
- **Slanje zahteva za prijateljstvo**
- **Prihvatanje/odbijanje zahteva**
- **Upravljanje listom prijatelja**
- **Uklanjanje prijatelja**
- **Neo4j graph struktura** za efikasno praćenje odnosa

### 📝 Deljenje Sadržaja
- **Kreiranje objava** sa tekstom i fajlovima
- **Upload različitih tipova fajlova** (slike, dokumenti, itd.)
- **Komentarisanje objava**
- **Lajkovanje objava**
- **Brisanje svojih objava**

### 💬 Real-time Chat
- **Direktne poruke** između prijatelja
- **SignalR WebSocket konekcija** za instant poruke
- **Kreiranje chat grupa** automatski kada se prihvati prijateljstvo
- **Status notifikacije** za nove poruke

### 🔍 Pretraga i Otkrivanje
- **Pretraga korisnika** po imenu
- **Filtriranje po statusu prijateljstva**
- **Pregled profila drugih korisnika**
- **Lista prijatelja** sa mogućnošću navigacije

### 📱 Responzivni Dizajn
- **Mobile-first pristup**
- **Vuetify Material Design** komponente
- **MATF brending** sa univerzitetskim bojama
- **Prilagodljivi layout** za sve uređaje

## 🚀 Pokretanje Aplikacije

### Preduslovi
```bash
# .NET 8 SDK
# Node.js 18+ sa npm/yarn
# SQL Server
# Neo4j Database
# RabbitMQ
# Git
```

### 1. Kloniranje Repozitorijuma
```bash
git clone https://github.com/neske99/MATF-drustvena-mreza
cd MATF-drustvena-mreza
```

### 2. Pokretanje Backend Servisa

#### Identity Service (Port 8094)
```bash
cd Security/IdentityServer
dotnet restore
dotnet run
```

#### Post Service (Port 8080)
```bash
cd Services/Post/Post.API
dotnet restore
dotnet run
```

#### Relations Service (Port 8000)
```bash
cd Services/Relations/Relations.API
dotnet restore
dotnet run
```

#### Chat Service (Port 8095)
```bash
cd Services/Chat/Chat.API
dotnet restore
dotnet run
```

### 3. Pokretanje Frontend Aplikacije
```bash
cd WebApp
npm install
# ili yarn install

npm run dev
# ili yarn dev
```

### 4. Konfiguracija Baza Podataka

#### SQL Server Connection Strings
Dodajte connection stringove u `appsettings.json` fajlove servisa:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MatfSocialNetwork;Trusted_Connection=true;"
  }
}
```

#### Neo4j Konfiguracija
```json
{
  "Neo4jSettings": {
    "Uri": "bolt://localhost:7687",
    "Username": "neo4j",
    "Password": "your-password"
  }
}
```

#### RabbitMQ Konfiguracija
```json
{
  "EventBusSettings": {
    "HostAddress": "amqp://guest:guest@localhost:5672"
  }
}
```

## 📁 Struktura Projekta

```
MATF-drustvena-mreza/
├── Common/                     # Zajednički moduli
│   ├── Common.EventBus/       # Event bus abstrakcije
│   └── Common.Logger/         # Logging konfiguracija
├── Security/
│   └── IdentityServer/        # Identity & Authentication servis
├── Services/
│   ├── Post/                  # Post mikroservis
│   │   ├── Post.API/         # Web API sloj
│   │   ├── Post.Application/ # Aplikaciona logika
│   │   ├── Post.Domain/      # Domain modeli
│   │   └── Post.Infrastructure/ # Data pristup
│   ├── Relations/            # Relations mikroservis
│   │   ├── Relations.API/
│   │   ├── Relations.Common/
│   │   └── Relations.GRPC/
│   └── Chat/                 # Chat mikroservis
│       ├── Chat.API/
│       ├── Chat.Model/
│       ├── Chat.Repository/
│       └── Chat.Service/
└── WebApp/                   # Vue.js frontend aplikacija
    ├── src/
    │   ├── components/      # Vue komponente
    │   ├── views/          # Stranice (Views)
    │   ├── stores/         # Pinia state management
    │   ├── services/       # API servisi
    │   └── dtos/          # TypeScript tipovi
    └── package.json
```

## 🔧 Konfiguracija Portova

| Servis | Port | Opis |
|--------|------|------|
| Identity Service | 8094 | Autentifikacija i korisnici |
| Post Service | 8080 | Objave i komentari |
| Relations Service | 8000 | Prijateljstva i odnosi |
| Chat Service | 8095 | Real-time chat |
| Vue.js App | 5173 | Frontend aplikacija (dev) |

## 📊 API Dokumentacija

Svi servisi imaju Swagger dokumentaciju dostupnu na:
- Identity: `http://localhost:8094/swagger`
- Post: `http://localhost:8080/swagger`  
- Relations: `http://localhost:8000/swagger`
- Chat: `http://localhost:8095/swagger`

## 🔐 Autentifikacija

Aplikacija koristi **JWT Bearer token** autentifikaciju sa:
- Access token (kratkotrajna validnost)
- Refresh token (dugotrajna validnost)
- Automatska obnova tokena
- Logout funkcionalnost

## 🎨 UI/UX Dizajn

Aplikacija koristi **MATF brending** sa:
- Crvena boja (`#8B0000`) kao primarna
- Material Design principi kroz Vuetify
- Responzivni dizajn za sve uređaje
- Animacije i tranzicije za bolji UX


## 🤝 Doprinos

Projekat je razvijen za potrebe Matematičkog fakulteta. Za predloge i poboljšanja, molimo da otvorite issue ili pošaljete pull request.

## 📄 Licenca

Ovaj projekat je kreiran u edukacione svrhe za Matematički fakultet Univerziteta u Beogradu.

## 👥 Tim

Razvijeno od strane studenata Matematičkog fakulteta kao deo projektnog zadatka.

---