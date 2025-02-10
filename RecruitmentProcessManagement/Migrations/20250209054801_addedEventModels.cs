using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentProcessManagement.Migrations
{
    /// <inheritdoc />
    public partial class addedEventModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_Candidates_CandidateID",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_AspNetUsers_InterviewerID",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Candidates_LinkedCandidateID",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_InterviewerID",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "InterviewerID",
                table: "Interviews");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Positions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ReasonForClosure",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Positions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "YearsOfExperience",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "InterviewRounds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "CandidateID",
                table: "DocumentVerifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileStatus",
                table: "Candidates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CollegeName",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtractedText",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventOrganizerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalParticipants = table.Column<int>(type: "int", nullable: false),
                    EventStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_EventOrganizerID",
                        column: x => x.EventOrganizerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Interviewers",
                columns: table => new
                {
                    InterviewerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviewers", x => x.InterviewerID);
                });

            migrationBuilder.CreateTable(
                name: "EventsCandidates",
                columns: table => new
                {
                    EventCandidateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    CandidateID = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsCandidates", x => x.EventCandidateID);
                    table.ForeignKey(
                        name: "FK_EventsCandidates_Candidates_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "Candidates",
                        principalColumn: "CandidateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventsCandidates_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventsInterviewers",
                columns: table => new
                {
                    EventInterviewerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    InterviewerID = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsInterviewers", x => x.EventInterviewerID);
                    table.ForeignKey(
                        name: "FK_EventsInterviewers_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventsInterviewers_Interviewers_InterviewerID",
                        column: x => x.InterviewerID,
                        principalTable: "Interviewers",
                        principalColumn: "InterviewerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewerSkills",
                columns: table => new
                {
                    InterviewerSkillID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewerID = table.Column<int>(type: "int", nullable: false),
                    SkillID = table.Column<int>(type: "int", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewerSkills", x => x.InterviewerSkillID);
                    table.ForeignKey(
                        name: "FK_InterviewerSkills_Interviewers_InterviewerID",
                        column: x => x.InterviewerID,
                        principalTable: "Interviewers",
                        principalColumn: "InterviewerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewerSkills_Skills_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skills",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewRoundInterviewers",
                columns: table => new
                {
                    RoundInterviewerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewRoundID = table.Column<int>(type: "int", nullable: false),
                    InterviewerID = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewRoundInterviewers", x => x.RoundInterviewerID);
                    table.ForeignKey(
                        name: "FK_InterviewRoundInterviewers_InterviewRounds_InterviewRoundID",
                        column: x => x.InterviewRoundID,
                        principalTable: "InterviewRounds",
                        principalColumn: "InterviewRoundID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewRoundInterviewers_Interviewers_InterviewerID",
                        column: x => x.InterviewerID,
                        principalTable: "Interviewers",
                        principalColumn: "InterviewerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventOrganizerID",
                table: "Events",
                column: "EventOrganizerID");

            migrationBuilder.CreateIndex(
                name: "IX_EventsCandidates_CandidateID",
                table: "EventsCandidates",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_EventsCandidates_EventID",
                table: "EventsCandidates",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventsInterviewers_EventID",
                table: "EventsInterviewers",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventsInterviewers_InterviewerID",
                table: "EventsInterviewers",
                column: "InterviewerID");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewerSkills_InterviewerID",
                table: "InterviewerSkills",
                column: "InterviewerID");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewerSkills_SkillID",
                table: "InterviewerSkills",
                column: "SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewRoundInterviewers_InterviewerID",
                table: "InterviewRoundInterviewers",
                column: "InterviewerID");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewRoundInterviewers_InterviewRoundID",
                table: "InterviewRoundInterviewers",
                column: "InterviewRoundID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_Candidates_CandidateID",
                table: "DocumentVerifications",
                column: "CandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Candidates_LinkedCandidateID",
                table: "Positions",
                column: "LinkedCandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVerifications_Candidates_CandidateID",
                table: "DocumentVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Candidates_LinkedCandidateID",
                table: "Positions");

            migrationBuilder.DropTable(
                name: "EventsCandidates");

            migrationBuilder.DropTable(
                name: "EventsInterviewers");

            migrationBuilder.DropTable(
                name: "InterviewerSkills");

            migrationBuilder.DropTable(
                name: "InterviewRoundInterviewers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Interviewers");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "InterviewRounds");

            migrationBuilder.DropColumn(
                name: "CollegeName",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ExtractedText",
                table: "Candidates");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ReasonForClosure",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Positions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewerID",
                table: "Interviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "CandidateID",
                table: "DocumentVerifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileStatus",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_InterviewerID",
                table: "Interviews",
                column: "InterviewerID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVerifications_Candidates_CandidateID",
                table: "DocumentVerifications",
                column: "CandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_AspNetUsers_InterviewerID",
                table: "Interviews",
                column: "InterviewerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Candidates_LinkedCandidateID",
                table: "Positions",
                column: "LinkedCandidateID",
                principalTable: "Candidates",
                principalColumn: "CandidateID");
        }
    }
}
