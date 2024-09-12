 JobPortal Documentation

Not: Authorization rahat test edilebilmesi için sadece örnek olarak UserController'da kullanıldı.

Proje: JobPortal
================

1\. Giriş
---------

**JobPortal**, iş ilanlarının yayınlanabileceği, şirketlerin ve kullanıcıların kayıt olabileceği bir platformdur. Proje .NET 7 üzerinde geliştirilmiş olup PostgreSQL, Redis ve Elasticsearch gibi teknolojilerle desteklenmiştir.

2\. Geliştirme Ortamı Gereksinimleri
------------------------------------

* .NET 7 SDK
* Docker (min. v20.10)
* PostgreSQL (Docker üzerinden)
* Elasticsearch (v7.10.1)
* Redis (alpine)
* Kibana (v7.10.1 - isteğe bağlı)

3\. Proje Yapısı
----------------

Proje aşağıdaki katmanlardan oluşmaktadır:

1.  **JobPortal.WebApi:** API katmanı, tüm isteklerin geldiği noktadır. Kullanıcı, şirket ve iş ilanı işlemleri burada yönetilir.
2.  **JobPortal.Business:** İş mantığının yazıldığı servis katmanıdır.
3.  **JobPortal.Data:** Veritabanı işlemleri ve EF Core ile kullanılan repository ve unit of work desenleri burada yer alır.
4.  **JobPortal.Common:** Genel yardımcı sınıflar, hata mesajları, JWT ve diğer genel işlevler burada tutulur.

4\. Bağımlılıklar ve Entegrasyonlar
-----------------------------------

### 4.1. PostgreSQL Entegrasyonu

PostgreSQL, veritabanı olarak kullanılmaktadır. Docker-compose ile PostgreSQL'i ayağa kaldırmak için aşağıdaki yapılandırma dosyası kullanılır:

    
    postgres:
      image: postgres:latest
      container_name: postgres_db
      restart: always
      environment:
        POSTGRES_USER: jobportaldbuser
        POSTGRES_PASSWORD: 35S_Ith??h3trEs8ucr@
        POSTGRES_DB: JobPortalDb
      ports:
        - "5432:5432"
      volumes:
        - postgres_data:/var/lib/postgresql/data
        

EF Core ile PostgreSQL bağlantısı sağlanır ve veritabanı işlemleri aşağıdaki şekilde yapılandırılır:

    
    builder.Services.AddDbContext<JobPortalDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        

### 4.2. Redis Entegrasyonu

Redis, hızlı erişim gerektiren bazı verilerin saklanması için kullanılmaktadır. Aşağıdaki gibi Docker üzerinden Redis servisi başlatılır:

    
    redis:
      image: redis:alpine
      container_name: redis
      ports:
        - "6379:6379"
      volumes:
        - redis_data:/data
        

Redis bağlantısı aşağıdaki gibi yapılır:

    
    builder.Services.AddSingleton(new RedisConnectionService(builder.Configuration.GetConnectionString("RedisConnection")));
        

### 4.3. Elasticsearch Entegrasyonu

Elasticsearch, iş ilanlarının tam metin arama ve filtreleme işlemleri için kullanılır. Docker-compose ile Elasticsearch aşağıdaki şekilde başlatılır:

    
    elasticsearch:
      image: docker.elastic.co/elasticsearch/elasticsearch:7.10.1
      container_name: elasticsearch
      environment:
        - discovery.type=single-node
        - ES_JAVA_OPTS=-Xms512m -Xmx512m
      ports:
        - "9200:9200"
        - "9300:9300"
        

Elasticsearch bağlantısı .NET Core'da şöyle yapılır:

    
    builder.Services.AddSingleton<IElasticClient>(sp =>
    {
        var settings = new ConnectionSettings(new Uri(builder.Configuration["ElasticSearch:Uri"]));
        return new ElasticClient(settings);
    });
        

5\. Kurulum Talimatları
-----------------------

1.  **Projeyi Klonlayın:**
    
        
        git clone https://github.com/username/jobportal.git
        cd jobportal
                
    
2.  **Docker Servislerini Başlatın:**
    
    Docker-compose ile PostgreSQL, Redis ve Elasticsearch’i başlatmak için:
    
        
        docker-compose up -d
                
    
3.  **Migration ve Veritabanı Güncellemesi:**
    
    Proje içerisinde veritabanını hazırlamak için migrationları çalıştırın:
    
        
        dotnet ef migrations add InitialMigration --project JobPortal.Data --startup-project JobPortal.WebApi
        dotnet ef database update --project JobPortal.Data --startup-project JobPortal.WebApi
                
    
4.  **Uygulamayı Çalıştırın:**
    
    Uygulamanın API katmanını çalıştırmak için:
    
        
        dotnet run --project JobPortal.WebApi
                
    
    Swagger arayüzü ile API'yi test edebilirsiniz: `http://localhost:5000/swagger`
    

6\. Proje İçindeki Önemli Yapılandırmalar
-----------------------------------------

### 6.1. appsettings.json

Proje için bağlantı dizeleri ve diğer yapılandırmalar `appsettings.json` dosyasında belirtilmiştir:

    
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=postgres_db;Port=5432;Database=JobPortalDb;Username=jobportaldbuser;Password=35S_Ith??h3trEs8ucr@",
        "RedisConnection": "redis:6379"
      },
      "ElasticsearchSettings": {
        "Uri": "http://localhost:9200"
      },
      "JwtSettings": {
        "Issuer": "JobPortal",
        "Audience": "JobPortalAudience",
        "SecretKey": "Cr6ThiCh0GLp$Af4iZ4tH_j1YLVo?a40",
        "TokenExpiryInMinutes": 60
      }
    }
        

7\. API Kullanımı
-----------------

### 7.1. Kullanıcı Kaydı

* **Endpoint:** `/api/users/register`
* **Yöntem:** `POST`
* **Örnek İstek:**

    
    {
      "name": "John Doe",
      "email": "john.doe@example.com",
      "password": "password123"
    }
        

### 7.2. İş İlanı Oluşturma

* **Endpoint:** `/api/jobs`
* **Yöntem:** `POST`
* **Örnek İstek:**

    
    {
      "companyId": 1,
      "position": "Backend Developer",
      "description": "Looking for an experienced .NET Developer.",
      "expirationDate": "2024-12-31"
    }
        
