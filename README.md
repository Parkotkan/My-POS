# My_POS - ระบบบริหารจัดการ การขาย (Point of Sale)

ระบบ POS ที่พัฒนาด้วย Frontend (Vue 3) และ Backend (ASP.NET Core 8.0)

## โครงสร้าง

```
My_POS/
├── Backend_API/          # ส่วนหลังบ้าน (ASP.NET Core 8.0 API)
│   ├── Controllers/      # ตัวจัดการ API
│   ├── Data/             # ส่วนเชื่อมต่อฐานข้อมูล (DbContext)
│   ├── Models/           # โมเดลข้อมูล
│   ├── Migrations/       # การตั้งค่าฐานข้อมูล (EF Core)
│   └── README.md         # เอกสารส่วน Backend
│
├── Frontend-vue/         # ส่วนหน้าบ้าน (Vue 3 + Vite)
│   ├── src/
│   │   ├── components/   # คอมโพเนนต์ที่ใช้ซ้ำ (เช่น Modern Modal)
│   │   ├── views/        # หน้าจอหลักของระบบ
│   │   └── css/          # ไฟล์สไตล์ (CSS)
│   ├── public/           # ไฟล์คงที่ (Static files)
│   ├── package.json      # การตั้งค่า dependencies
│   ├── vite.config.js    # การตั้งค่า Vite
│   └── README.md         # เอกสารส่วน Frontend
│
└── My_POS.sln            # ไฟล์ Solution สำหรับ Visual Studio
```

## สถาปัตยกรรม (Architecture)

โปรเจกต์นี้ใช้สถาปัตยกรรมแบบแยก Client และ Server ออกจากกันอย่างอิสระ:

### Backend (หลังบ้าน)
- **เทคโนโลยี**: ASP.NET Core 8.0
- **พอร์ต (Port)**: 5000 (HTTP), 5001 (HTTPS)
- **ฐานข้อมูล**: SQL Server
- **หน้าที่**: RESTful API

### Frontend (หน้าบ้าน)
- **เทคโนโลยี**: Vue 3 พร้อม Vite
- **พอร์ต (Port)**: 3000
- **หน้าที่**: ส่วนติดต่อผู้ใช้

### การสื่อสาร
- Frontend เรียกใช้ API ผ่าน HTTP/CORS
- ใช้ตัวแปรสภาพแวดล้อม (.env) ในการตั้งค่า URL ของ API
- การพัฒนาและการปรับใช้งาน (Deployment) แยกจากกัน

## เริ่มต้นใช้งาน

### การตั้งค่า Backend

```bash
cd Backend_API

# ติดตั้ง dependencies
dotnet restore

# อัปเดตฐานข้อมูล
dotnet ef database update

# รันระบบหลังบ้าน
dotnet run
```

Backend จะพร้อมใช้งานที่: `http://localhost:5000/api`

### การตั้งค่า Frontend

```bash
cd Frontend-vue

# ติดตั้ง dependencies
npm install

# รันระบบหน้าบ้านสำหรับพัฒนา
npm run dev
```

Frontend จะพร้อมใช้งานที่: `http://localhost:3000`

## 📦 การติดตั้งและพัฒนา

### สิ่งที่ต้องมี (Prerequisites)
- **.NET 8.0 SDK** - [ดาวน์โหลด](https://dotnet.microsoft.com/download)
- **Node.js v16+** - [ดาวน์โหลด](https://nodejs.org/)
- **SQL Server** - Instance ท้องถิ่นหรือ Connection String ที่กำหนดค่าไว้

### ขั้นตอนที่ 1: ตั้งค่า Backend

1. แก้ไขไฟล์ `Backend_API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=POS_DB;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

2. รัน Migrations:
```bash
cd Backend_API
dotnet ef database update
```

### ขั้นตอนที่ 2: ตั้งค่า Frontend

1. ติดตั้ง dependencies:
```bash
cd Frontend-vue
npm install
```

2. สร้างไฟล์ `.env`:
```bash
# คัดลอกฐานจากไฟล์ตัวอย่าง
cp .env.example .env
```

## 📚 คุณสมบัติเด่น (Features)

### หน้าจอขายสินค้า (POS Terminal)
- 🛒 ค้นหาสินค้าและเพิ่มลงตะกร้าอย่างรวดเร็ว
- 📊 คำนวณราคารวมแบบเรียลไทม์
- 👥 เลือกแคชเชียร์ผู้ทำรายการ
- ✨ **ใหม่!** ระบบแจ้งเตือน (Alert) แบบ Modern Modal สวยงาม
- ✅ ระบบชำระเงินและตรวจสอบความถูกต้องของข้อมูล

### การจัดการสินค้า (Product Management)
- ดูรายการสินค้าทั้งหมด
- จัดการสต็อกและราคาสินค้า
- แยกหมวดหมู่สินค้า

### การจัดการคำสั่งซื้อ (Order Management)
- บันทึกประวัติการขาย
- **ใหม่!** แสดงรหัสสั่งซื้อและ **ชื่อผู้ขาย (Seller Name)** ในตารางสรุป
- ดูรายละเอียดรายการสินค้าในแต่ละคำสั่งซื้อ

### การจัดการผู้ใช้ (User Management)
- จัดการบัญชีพนักงาน/แคชเชียร์
- ติดตามผลงานการขายของพนักงานแต่ละคน

## 🔌 จุดเชื่อมต่อ API (API Endpoints)

### สินค้า (Products)
- `GET /api/products` - ดึงข้อมูลสินค้าทั้งหมด
- `POST /api/products` - เพิ่มสินค้าใหม่
- `DELETE /api/products/{id}` - ลบสินค้า

### คำสั่งซื้อ (Orders)
- `GET /api/orders` - ดึงประวัติการสั่งซื้อ (รวมข้อมูลผู้ขาย)
- `POST /api/orders` - สร้างคำสั่งซื้อใหม่ (พร้อมคำนวณราคารวมอัตโนมัติ)

## 📊 รายละเอียดฐานข้อมูล (Schema)

- **Users**: ข้อมูลพนักงาน (Username, ชื่อ-นามสกุล, บทบาท)
- **Products**: ข้อมูลสินค้า (ชื่อ, ราคา, หมวดหมู่, สถานะ)
- **Orders**: ข้อมูลการขาย (วันที่, ราคารวม, ผู้ขาย)
- **Order_Items**: รายละเอียดสินค้าในแต่ละบิล (สินค้า, จำนวน, ราคาต่อหน่วย)

## ⚙️ ตัวแปรสภาพแวดล้อม (Environment Variables)

### Frontend (`.env`)
```
VITE_API_URL=http://localhost:5000
```

## 📝 บันทึกการอัปเดตล่าสุด (Recent Updates)

1.  **Backend Validation**: ปรับปรุงการตรวจสอบข้อมูลฝั่ง Server ให้ยืดหยุ่นขึ้น (รองรับการคำนวณราคาก่อนบันทึก)
2.  **Modern UI**: เพิ่มคอมโพเนนต์ `BaseModal` เพื่อใช้แทนการแจ้งเตือนแบบ Alert เดิมของบราวเซอร์
3.  **Enhanced Orders**: หน้าประวัติการสั่งซื้อแสดงรหัสและชื่อพนักงานผู้ขายอย่างชัดเจน
4.  **Centralized Config**: ปรับปรุงให้ทุกหน้าเรียกใช้ API URL จากไฟล์ `.env` เพียงที่เดียว

---

**อัปเดตล่าสุด**: มิถุนายน 2026  
**สถานะโปรเจกต์**: ยังไม่เสร็จสมบูรณ์ ต้องการเพิ่ม การ login , กำหนดสิทธิ์การเข้าถึงสำหรับตำแหน่งต่างๆ , การเพิ่ม ลบ แก้ไขสินค้า , การแสดงรายละเอียดคำสั่งซื้อ

