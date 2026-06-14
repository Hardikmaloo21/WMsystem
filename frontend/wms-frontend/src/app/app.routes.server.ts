import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'employees/edit/:id',
    renderMode: RenderMode.Client
  },
  {
    path: 'employees/profile/:id',
    renderMode: RenderMode.Client
  },
  {
    path: 'departments/edit/:id',
    renderMode: RenderMode.Client
  },
  {
    path: 'projects/edit/:id',
    renderMode: RenderMode.Client
  },
  {
    path: 'clients/edit/:id',
    renderMode: RenderMode.Client
  },
  {
    path: 'allocations/edit/:id',
    renderMode: RenderMode.Client
  },
  {
    path: 'announcements/edit/:id',
    renderMode: RenderMode.Client
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];