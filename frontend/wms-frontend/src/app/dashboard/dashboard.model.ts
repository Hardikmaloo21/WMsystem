export interface AttendanceTrendItem {
  date: string;
  present?: number;
  absent?: number;
  wfo?: number;
  wfh?: number;
  wFO?: number;
  wFH?: number;
}

export interface DepartmentDistributionItem {
  departmentName: string;
  employeeCount: number;
}

export interface LeaveTrendItem {
  month: string;
  approved: number;
  pending: number;
  rejected: number;
}

export interface ProjectStatusDistributionItem {
  status: string;
  count: number;
}

export interface RecentAnnouncementDto {
  announcementId: number;
  title: string;
  message: string;
  createdByName: string;
  createdOn: string;
}

export interface DashboardSummaryDto {
  totalEmployees: number;
  activeEmployees: number;
  totalDepartments: number;
  totalProjects: number;
  activeProjects: number;
  totalClients: number;
  todayAttendanceCount: number;
  pendingLeaves: number;

  attendanceTrend: AttendanceTrendItem[];
  departmentDistribution: DepartmentDistributionItem[];
  leaveTrend: LeaveTrendItem[];
  projectStatusDistribution: ProjectStatusDistributionItem[];

  recentAnnouncements: RecentAnnouncementDto[];
}

