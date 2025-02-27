--
-- PostgreSQL database dump
--

-- Dumped from database version 16.6
-- Dumped by pg_dump version 16.6

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Admins; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Admins" (
    "AdminId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "Department" character varying(50) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Admins" OWNER TO funghu;

--
-- Name: Admins_AdminId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Admins" ALTER COLUMN "AdminId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Admins_AdminId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Branches; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Branches" (
    "BranchId" integer NOT NULL,
    "CenterId" integer NOT NULL,
    "BranchName" character varying(100) NOT NULL,
    "Address" text NOT NULL,
    "Phone" character varying(20) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Branches" OWNER TO funghu;

--
-- Name: Branches_BranchId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Branches" ALTER COLUMN "BranchId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Branches_BranchId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: DevelopmentRecords; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."DevelopmentRecords" (
    "RecordId" integer NOT NULL,
    "StudentId" integer NOT NULL,
    "RecordDate" timestamp with time zone NOT NULL,
    "Height" numeric(5,2) NOT NULL,
    "Weight" numeric(5,2) NOT NULL,
    "InstructorComments" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."DevelopmentRecords" OWNER TO funghu;

--
-- Name: DevelopmentRecords_RecordId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."DevelopmentRecords" ALTER COLUMN "RecordId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."DevelopmentRecords_RecordId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Enrollments; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Enrollments" (
    "EnrollmentId" integer NOT NULL,
    "StudentId" integer NOT NULL,
    "SessionId" integer NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Enrollments" OWNER TO funghu;

--
-- Name: Enrollments_EnrollmentId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Enrollments" ALTER COLUMN "EnrollmentId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Enrollments_EnrollmentId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: InstructorBranches; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."InstructorBranches" (
    "InstructorBranchId" integer NOT NULL,
    "InstructorId" integer NOT NULL,
    "BranchId" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."InstructorBranches" OWNER TO funghu;

--
-- Name: InstructorBranches_InstructorBranchId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."InstructorBranches" ALTER COLUMN "InstructorBranchId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."InstructorBranches_InstructorBranchId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Instructors; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Instructors" (
    "InstructorId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "Specialization" character varying(100) NOT NULL,
    "HireDate" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Instructors" OWNER TO funghu;

--
-- Name: Instructors_InstructorId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Instructors" ALTER COLUMN "InstructorId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Instructors_InstructorId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Parents; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Parents" (
    "ParentId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "EmergencyContact" character varying(100) NOT NULL,
    "EmergencyPhone" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Parents" OWNER TO funghu;

--
-- Name: Parents_ParentId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Parents" ALTER COLUMN "ParentId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Parents_ParentId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: PaymentPlans; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."PaymentPlans" (
    "PlanId" integer NOT NULL,
    "EnrollmentId" integer NOT NULL,
    "TotalAmount" numeric(10,2) NOT NULL,
    "NumberOfInstallments" integer NOT NULL,
    "PaymentDay" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."PaymentPlans" OWNER TO funghu;

--
-- Name: PaymentPlans_PlanId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."PaymentPlans" ALTER COLUMN "PlanId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."PaymentPlans_PlanId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Payments; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Payments" (
    "PaymentId" integer NOT NULL,
    "PlanId" integer NOT NULL,
    "Amount" numeric(10,2) NOT NULL,
    "DueDate" timestamp with time zone NOT NULL,
    "PaymentDate" timestamp with time zone,
    "Status" integer NOT NULL,
    "PaymentMethod" character varying(50),
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Payments" OWNER TO funghu;

--
-- Name: Payments_PaymentId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Payments" ALTER COLUMN "PaymentId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Payments_PaymentId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Sessions; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Sessions" (
    "SessionId" integer NOT NULL,
    "BranchId" integer NOT NULL,
    "InstructorId" integer NOT NULL,
    "SessionName" character varying(100) NOT NULL,
    "StartTime" interval NOT NULL,
    "EndTime" interval NOT NULL,
    "DayOfWeek" integer NOT NULL,
    "MaxCapacity" integer NOT NULL,
    "Status" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Sessions" OWNER TO funghu;

--
-- Name: Sessions_SessionId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Sessions" ALTER COLUMN "SessionId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Sessions_SessionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SportsCenters; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."SportsCenters" (
    "CenterId" integer NOT NULL,
    "CenterName" character varying(100) NOT NULL,
    "Address" text NOT NULL,
    "Phone" character varying(20) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."SportsCenters" OWNER TO funghu;

--
-- Name: SportsCenters_CenterId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."SportsCenters" ALTER COLUMN "CenterId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."SportsCenters_CenterId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Students; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Students" (
    "StudentId" integer NOT NULL,
    "ParentId" integer NOT NULL,
    "FirstName" character varying(50) NOT NULL,
    "LastName" character varying(50) NOT NULL,
    "DateOfBirth" timestamp with time zone NOT NULL,
    "Gender" character(1) NOT NULL,
    "MedicalConditions" text,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Students" OWNER TO funghu;

--
-- Name: Students_StudentId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Students" ALTER COLUMN "StudentId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Students_StudentId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Users; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."Users" (
    "UserId" integer NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" integer NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "Phone" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Users" OWNER TO funghu;

--
-- Name: Users_UserId_seq; Type: SEQUENCE; Schema: public; Owner: funghu
--

ALTER TABLE public."Users" ALTER COLUMN "UserId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Users_UserId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: funghu
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO funghu;

--
-- Data for Name: Admins; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Admins" ("AdminId", "UserId", "Department", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	Management	2024-12-31 15:49:52.101533+03	2024-12-31 15:49:52.101533+03
\.


--
-- Data for Name: Branches; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Branches" ("BranchId", "CenterId", "BranchName", "Address", "Phone", "Email", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	Tennis	teniis address	1238759435600	Tennis@sportcenter1.com	2024-12-31 15:59:59.940783+03	2024-12-31 15:59:59.940783+03
\.


--
-- Data for Name: DevelopmentRecords; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."DevelopmentRecords" ("RecordId", "StudentId", "RecordDate", "Height", "Weight", "InstructorComments", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: Enrollments; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Enrollments" ("EnrollmentId", "StudentId", "SessionId", "StartDate", "EndDate", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	1	2024-12-31 00:00:00+03	2025-01-31 00:00:00+03	t	2024-12-31 16:14:40.216833+03	2024-12-31 16:14:40.216833+03
\.


--
-- Data for Name: InstructorBranches; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."InstructorBranches" ("InstructorBranchId", "InstructorId", "BranchId", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	1	2024-12-31 16:10:15.255289+03	2024-12-31 16:10:15.255289+03
\.


--
-- Data for Name: Instructors; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Instructors" ("InstructorId", "UserId", "Specialization", "HireDate", "CreatedAt", "UpdatedAt") FROM stdin;
1	2	Tennis	2024-12-31 03:00:00+03	2024-12-31 16:10:15.238737+03	2024-12-31 16:10:15.238737+03
\.


--
-- Data for Name: Parents; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Parents" ("ParentId", "UserId", "EmergencyContact", "EmergencyPhone", "CreatedAt", "UpdatedAt") FROM stdin;
1	3	Haluk	123947534	2024-12-31 16:12:30.116919+03	2024-12-31 16:12:30.116919+03
\.


--
-- Data for Name: PaymentPlans; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."PaymentPlans" ("PlanId", "EnrollmentId", "TotalAmount", "NumberOfInstallments", "PaymentDay", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	10000.00	6	6	2024-12-31 16:14:54.986081+03	2024-12-31 16:14:54.986081+03
\.


--
-- Data for Name: Payments; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Payments" ("PaymentId", "PlanId", "Amount", "DueDate", "PaymentDate", "Status", "PaymentMethod", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	1666.67	2025-01-05 03:00:00+03	\N	0	\N	2024-12-31 16:14:55.003989+03	2024-12-31 16:14:55.003989+03
2	1	1666.67	2025-02-05 03:00:00+03	\N	0	\N	2024-12-31 16:14:55.01705+03	2024-12-31 16:14:55.01705+03
3	1	1666.67	2025-03-05 03:00:00+03	\N	0	\N	2024-12-31 16:14:55.01713+03	2024-12-31 16:14:55.01713+03
4	1	1666.67	2025-04-05 03:00:00+03	\N	0	\N	2024-12-31 16:14:55.017147+03	2024-12-31 16:14:55.017147+03
5	1	1666.67	2025-05-05 03:00:00+03	\N	0	\N	2024-12-31 16:14:55.017178+03	2024-12-31 16:14:55.017178+03
6	1	1666.65	2025-06-05 03:00:00+03	\N	0	\N	2024-12-31 16:14:55.017196+03	2024-12-31 16:14:55.017196+03
\.


--
-- Data for Name: Sessions; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Sessions" ("SessionId", "BranchId", "InstructorId", "SessionName", "StartTime", "EndTime", "DayOfWeek", "MaxCapacity", "Status", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	1	Tennis with coach1	00:00:00	12:05:00	2	6	0	2024-12-31 16:10:58.522622+03	2024-12-31 16:10:58.522622+03
\.


--
-- Data for Name: SportsCenters; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."SportsCenters" ("CenterId", "CenterName", "Address", "Phone", "Email", "CreatedAt", "UpdatedAt") FROM stdin;
1	Sportcenter1	Sportcenter address	231548712378	Sportcenter1@sportcenter.com	2024-12-31 15:53:08.58549+03	2024-12-31 15:53:08.58549+03
\.


--
-- Data for Name: Students; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Students" ("StudentId", "ParentId", "FirstName", "LastName", "DateOfBirth", "Gender", "MedicalConditions", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
1	1	Child1	child	1963-06-26 03:00:00+03	M	Missing toe	t	2024-12-31 16:14:25.391971+03	2024-12-31 16:14:25.391971+03
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."Users" ("UserId", "Email", "PasswordHash", "Role", "FirstName", "LastName", "Phone", "IsActive", "CreatedAt", "UpdatedAt") FROM stdin;
1	manage@manage.com	AQAAAAIAAYagAAAAEFGJTnPEIP5cEQREEzV1itxF/urBGtqLdew8GunpIoo9R1hpMmL4HP0GCvQcv+uLXQ==	0	manage	manager	123456789	t	2024-12-31 15:49:51.880399+03	2024-12-31 15:49:51.880399+03
2	coach1@coach.com	AQAAAAIAAYagAAAAEN+2MxPAyEYEL0bL7mw+zOp48hdwdawTu0OAUvNUB3a3nzM8d5Bqk7q6mD6J8T8J+g==	1	Coach1	coach	1234567987	t	2024-12-31 16:10:15.164108+03	2024-12-31 16:10:15.164108+03
3	parent1@parent.com	AQAAAAIAAYagAAAAEOS+nRVPFpUR7SDm7/8vs9OLyhgtv+rQQIHbeeZTB+0BUi00cZ6BC9TynbyPy7Aerg==	2	Parent1	parent	1235623432	t	2024-12-31 16:12:30.047308+03	2024-12-31 16:12:30.047308+03
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: funghu
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20241215094742_InitialCreate	8.0.0
20241215111438_ParentAndStudentModels	8.0.0
20241215114222_UpdateDateTimeHandling	8.0.0
20241217111032_InstructorModel	8.0.0
20241217120626_AdminEntity	8.0.0
20241222112419_paymentAndSessions	8.0.0
20241222114158_AddPaymentAndRelatedEntities	8.0.0
20241229125439_AddInstructorBranchManyToMany	8.0.0
20241229142230_AddDevelopmentRecords	8.0.0
20241231123202_AddStudentEnrollmentsRelationship	8.0.0
20241231123942_redoing	8.0.0
\.


--
-- Name: Admins_AdminId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Admins_AdminId_seq"', 1, true);


--
-- Name: Branches_BranchId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Branches_BranchId_seq"', 1, true);


--
-- Name: DevelopmentRecords_RecordId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."DevelopmentRecords_RecordId_seq"', 1, false);


--
-- Name: Enrollments_EnrollmentId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Enrollments_EnrollmentId_seq"', 1, true);


--
-- Name: InstructorBranches_InstructorBranchId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."InstructorBranches_InstructorBranchId_seq"', 1, true);


--
-- Name: Instructors_InstructorId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Instructors_InstructorId_seq"', 1, true);


--
-- Name: Parents_ParentId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Parents_ParentId_seq"', 1, true);


--
-- Name: PaymentPlans_PlanId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."PaymentPlans_PlanId_seq"', 1, true);


--
-- Name: Payments_PaymentId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Payments_PaymentId_seq"', 6, true);


--
-- Name: Sessions_SessionId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Sessions_SessionId_seq"', 1, true);


--
-- Name: SportsCenters_CenterId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."SportsCenters_CenterId_seq"', 1, true);


--
-- Name: Students_StudentId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Students_StudentId_seq"', 1, true);


--
-- Name: Users_UserId_seq; Type: SEQUENCE SET; Schema: public; Owner: funghu
--

SELECT pg_catalog.setval('public."Users_UserId_seq"', 3, true);


--
-- Name: Admins PK_Admins; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Admins"
    ADD CONSTRAINT "PK_Admins" PRIMARY KEY ("AdminId");


--
-- Name: Branches PK_Branches; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Branches"
    ADD CONSTRAINT "PK_Branches" PRIMARY KEY ("BranchId");


--
-- Name: DevelopmentRecords PK_DevelopmentRecords; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."DevelopmentRecords"
    ADD CONSTRAINT "PK_DevelopmentRecords" PRIMARY KEY ("RecordId");


--
-- Name: Enrollments PK_Enrollments; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Enrollments"
    ADD CONSTRAINT "PK_Enrollments" PRIMARY KEY ("EnrollmentId");


--
-- Name: InstructorBranches PK_InstructorBranches; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."InstructorBranches"
    ADD CONSTRAINT "PK_InstructorBranches" PRIMARY KEY ("InstructorBranchId");


--
-- Name: Instructors PK_Instructors; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Instructors"
    ADD CONSTRAINT "PK_Instructors" PRIMARY KEY ("InstructorId");


--
-- Name: Parents PK_Parents; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Parents"
    ADD CONSTRAINT "PK_Parents" PRIMARY KEY ("ParentId");


--
-- Name: PaymentPlans PK_PaymentPlans; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."PaymentPlans"
    ADD CONSTRAINT "PK_PaymentPlans" PRIMARY KEY ("PlanId");


--
-- Name: Payments PK_Payments; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "PK_Payments" PRIMARY KEY ("PaymentId");


--
-- Name: Sessions PK_Sessions; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "PK_Sessions" PRIMARY KEY ("SessionId");


--
-- Name: SportsCenters PK_SportsCenters; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."SportsCenters"
    ADD CONSTRAINT "PK_SportsCenters" PRIMARY KEY ("CenterId");


--
-- Name: Students PK_Students; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Students"
    ADD CONSTRAINT "PK_Students" PRIMARY KEY ("StudentId");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("UserId");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Admins_UserId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE UNIQUE INDEX "IX_Admins_UserId" ON public."Admins" USING btree ("UserId");


--
-- Name: IX_Branches_CenterId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Branches_CenterId" ON public."Branches" USING btree ("CenterId");


--
-- Name: IX_DevelopmentRecords_StudentId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_DevelopmentRecords_StudentId" ON public."DevelopmentRecords" USING btree ("StudentId");


--
-- Name: IX_Enrollments_SessionId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Enrollments_SessionId" ON public."Enrollments" USING btree ("SessionId");


--
-- Name: IX_Enrollments_StudentId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Enrollments_StudentId" ON public."Enrollments" USING btree ("StudentId");


--
-- Name: IX_InstructorBranches_BranchId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_InstructorBranches_BranchId" ON public."InstructorBranches" USING btree ("BranchId");


--
-- Name: IX_InstructorBranches_InstructorId_BranchId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE UNIQUE INDEX "IX_InstructorBranches_InstructorId_BranchId" ON public."InstructorBranches" USING btree ("InstructorId", "BranchId");


--
-- Name: IX_Instructors_UserId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE UNIQUE INDEX "IX_Instructors_UserId" ON public."Instructors" USING btree ("UserId");


--
-- Name: IX_Parents_UserId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE UNIQUE INDEX "IX_Parents_UserId" ON public."Parents" USING btree ("UserId");


--
-- Name: IX_PaymentPlans_EnrollmentId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE UNIQUE INDEX "IX_PaymentPlans_EnrollmentId" ON public."PaymentPlans" USING btree ("EnrollmentId");


--
-- Name: IX_Payments_PlanId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Payments_PlanId" ON public."Payments" USING btree ("PlanId");


--
-- Name: IX_Sessions_BranchId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Sessions_BranchId" ON public."Sessions" USING btree ("BranchId");


--
-- Name: IX_Sessions_InstructorId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Sessions_InstructorId" ON public."Sessions" USING btree ("InstructorId");


--
-- Name: IX_Students_ParentId; Type: INDEX; Schema: public; Owner: funghu
--

CREATE INDEX "IX_Students_ParentId" ON public."Students" USING btree ("ParentId");


--
-- Name: IX_Users_Email; Type: INDEX; Schema: public; Owner: funghu
--

CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");


--
-- Name: Admins FK_Admins_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Admins"
    ADD CONSTRAINT "FK_Admins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId") ON DELETE RESTRICT;


--
-- Name: Branches FK_Branches_SportsCenters_CenterId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Branches"
    ADD CONSTRAINT "FK_Branches_SportsCenters_CenterId" FOREIGN KEY ("CenterId") REFERENCES public."SportsCenters"("CenterId") ON DELETE CASCADE;


--
-- Name: DevelopmentRecords FK_DevelopmentRecords_Students_StudentId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."DevelopmentRecords"
    ADD CONSTRAINT "FK_DevelopmentRecords_Students_StudentId" FOREIGN KEY ("StudentId") REFERENCES public."Students"("StudentId") ON DELETE RESTRICT;


--
-- Name: Enrollments FK_Enrollments_Sessions_SessionId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Enrollments"
    ADD CONSTRAINT "FK_Enrollments_Sessions_SessionId" FOREIGN KEY ("SessionId") REFERENCES public."Sessions"("SessionId") ON DELETE CASCADE;


--
-- Name: Enrollments FK_Enrollments_Students_StudentId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Enrollments"
    ADD CONSTRAINT "FK_Enrollments_Students_StudentId" FOREIGN KEY ("StudentId") REFERENCES public."Students"("StudentId") ON DELETE CASCADE;


--
-- Name: InstructorBranches FK_InstructorBranches_Branches_BranchId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."InstructorBranches"
    ADD CONSTRAINT "FK_InstructorBranches_Branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES public."Branches"("BranchId") ON DELETE RESTRICT;


--
-- Name: InstructorBranches FK_InstructorBranches_Instructors_InstructorId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."InstructorBranches"
    ADD CONSTRAINT "FK_InstructorBranches_Instructors_InstructorId" FOREIGN KEY ("InstructorId") REFERENCES public."Instructors"("InstructorId") ON DELETE RESTRICT;


--
-- Name: Instructors FK_Instructors_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Instructors"
    ADD CONSTRAINT "FK_Instructors_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId") ON DELETE RESTRICT;


--
-- Name: Parents FK_Parents_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Parents"
    ADD CONSTRAINT "FK_Parents_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId") ON DELETE RESTRICT;


--
-- Name: PaymentPlans FK_PaymentPlans_Enrollments_EnrollmentId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."PaymentPlans"
    ADD CONSTRAINT "FK_PaymentPlans_Enrollments_EnrollmentId" FOREIGN KEY ("EnrollmentId") REFERENCES public."Enrollments"("EnrollmentId") ON DELETE CASCADE;


--
-- Name: Payments FK_Payments_PaymentPlans_PlanId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "FK_Payments_PaymentPlans_PlanId" FOREIGN KEY ("PlanId") REFERENCES public."PaymentPlans"("PlanId") ON DELETE CASCADE;


--
-- Name: Sessions FK_Sessions_Branches_BranchId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "FK_Sessions_Branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES public."Branches"("BranchId") ON DELETE CASCADE;


--
-- Name: Sessions FK_Sessions_Instructors_InstructorId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "FK_Sessions_Instructors_InstructorId" FOREIGN KEY ("InstructorId") REFERENCES public."Instructors"("InstructorId") ON DELETE CASCADE;


--
-- Name: Students FK_Students_Parents_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: funghu
--

ALTER TABLE ONLY public."Students"
    ADD CONSTRAINT "FK_Students_Parents_ParentId" FOREIGN KEY ("ParentId") REFERENCES public."Parents"("ParentId") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

